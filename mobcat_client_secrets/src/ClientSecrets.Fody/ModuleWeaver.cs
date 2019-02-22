using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fody;
using MobCAT.ClientSecrets;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal enum FieldInitMethod
{
    ConstantInitialValue,
    PublicConstructor,
    StaticConstructor
}

public class ModuleWeaver : BaseModuleWeaver
{
    public override bool ShouldCleanReference => true;

    public override void Execute()
    {
        // Get types with fields decorated with the ClientSecret Attribute
        var typesWithSecrets = ModuleDefinition.Assembly.MainModule.GetTypes()
            .Where(i => i.Fields
                .Where(i2 => i2.CustomAttributes
                    .Where(i3 => i3.AttributeType.Name == nameof(ClientSecretAttribute))
                .Count() > 0)
            .Count() > 0)
            .ToList();

        // Process fields to initialize them using the replacement secret values
        foreach (var typeDefinition in typesWithSecrets)
        {
            // Identify the fields, within the parent type, that are decorated with the ClientSecret Attribute
            var fieldsToProcess = typeDefinition.Fields
                .Where(i => i.CustomAttributes.FirstOrDefault(i2 => i2.AttributeType.Name == nameof(ClientSecretAttribute)) != null);
            
            typeDefinition.IsBeforeFieldInit = false;

            // Set the constant and initial values for literal fields
            var literalFields = fieldsToProcess.Where(i => i.IsLiteral).ToList();           
            ProcessClientSecrets(typeDefinition, literalFields, FieldInitMethod.ConstantInitialValue);

            // Set static fields in the static constructor
            var staticFields = fieldsToProcess.Where(i => !i.IsLiteral && i.IsStatic).ToList();
            ProcessClientSecrets(typeDefinition, staticFields, FieldInitMethod.StaticConstructor);

            // Set all other fields in the public constructor
            var readonlyFields = fieldsToProcess.Where(i => !i.IsLiteral && (!i.IsStatic && i.IsInitOnly)).ToList();
            var standardFields = fieldsToProcess.Where(i => !i.IsLiteral && !i.IsStatic && !i.IsInitOnly).ToList();
            ProcessClientSecrets(typeDefinition, readonlyFields.Concat(standardFields).ToList(), FieldInitMethod.PublicConstructor);

            // Remove the ClientSecret Attribute from all processed fields
            RemoveClientSecretAttribute(fieldsToProcess);
        }
    }    

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
    }

    private void ProcessClientSecrets(TypeDefinition typeDefinition, IEnumerable<FieldDefinition> fields, FieldInitMethod fieldInitMethod)
    {
        if (fields.Count() == 0)
            return;

        if (fieldInitMethod == FieldInitMethod.ConstantInitialValue)
        {
            foreach (var field in fields)
            {
                var replacementFieldValue = ResolveEnvironmentVariableValue(ResolveFieldKey(field));
                field.Constant = replacementFieldValue;
                field.InitialValue = Encoding.UTF8.GetBytes(replacementFieldValue);
            }

            return;
        }

        var ctorAttributes = fieldInitMethod == FieldInitMethod.PublicConstructor ?
            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName : 
            MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Static;

        MethodDefinition ctor = typeDefinition.Methods.Where(i => i.Attributes == ctorAttributes).FirstOrDefault();

        // Update the constructor to initialize each field with the expected value        
        var il = ctor.Body.GetILProcessor();
        var returnOpCode = il.Body.Instructions.Last();
        var valueReplacementOpCode = fieldInitMethod == FieldInitMethod.PublicConstructor ? OpCodes.Stfld : OpCodes.Stsfld;
        
        foreach (var field in fields)
        {
            var fieldKey = ResolveFieldKey(field);
            var replacementFieldValue = ResolveEnvironmentVariableValue(fieldKey);

            // Loads the value at implicit argument index 0 onto the stack (for use with instance methods only)
            if (fieldInitMethod == FieldInitMethod.PublicConstructor)
                il.InsertBefore(returnOpCode, Instruction.Create(OpCodes.Ldarg_0));

            // Pushes a new object reference to a string literal (for use in the next instruction)
            il.InsertBefore(returnOpCode, Instruction.Create(OpCodes.Ldstr, replacementFieldValue));

            // Replaces the value of a static field (stsfld) or replaces the value stored in the field of an object reference (stfld) or pointer with a new value
            il.InsertBefore(returnOpCode, Instruction.Create(valueReplacementOpCode, field));
        }
    }

    private void RemoveClientSecretAttribute(IEnumerable<FieldDefinition> fields)
    {
        foreach (var field in fields)
        {
            var clientSecretAttribute = field.CustomAttributes.Where(i => i.AttributeType.Name.ToString() == nameof(ClientSecretAttribute)).FirstOrDefault();

            if (clientSecretAttribute != null)
                field.CustomAttributes.Remove(clientSecretAttribute);
        }
    }

    private string ResolveFieldKey(FieldDefinition field)
    {
        var clientSecretAttribute = field.CustomAttributes
            .Where(i => i.AttributeType.Name.ToString() == nameof(ClientSecretAttribute))
            .FirstOrDefault();

        // If no constructor argument is provided, use the name of the field as the key
        var fieldKey = clientSecretAttribute.HasConstructorArguments ? 
            clientSecretAttribute.ConstructorArguments.FirstOrDefault().Value.ToString() : 
            field.Name.ToString();

        return fieldKey;
    }

    private string ResolveEnvironmentVariableValue(string key)
    {
        string replacementFieldValue = string.Empty;

        if (string.IsNullOrWhiteSpace(replacementFieldValue))
            replacementFieldValue = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);

        if (string.IsNullOrWhiteSpace(replacementFieldValue))
            replacementFieldValue = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);

        if (string.IsNullOrWhiteSpace(replacementFieldValue))
            replacementFieldValue = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);

        return replacementFieldValue ?? string.Empty;
    }
}