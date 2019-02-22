# Overview
This [Fody](https://github.com/Fody/Fody/) add-in injects code for setting field values based on environment variables (resolved at build-time) aiding in keeping secrets out of the source code in a simple manner. 

# Getting Started

## NuGet Installation
Install the [MobCAT.ClientSecrets.Fody package](https://dotnetcst.visualstudio.com/MobCAT/_packaging?_a=package&feed=MobCAT-Internal&package=MobCAT.ClientSecrets.Fody&protocolType=NuGet
)

```
PM> Install-Package MobCAT.ClientSecrets.Fody
```

## Configure FodyWeavers.xml

Add **<MobCAT.ClientSecrets/>** to [FodyWeavers.xml](https://github.com/Fody/Fody#add-fodyweaversxml). This file must be added manually e.g.  

```
<?xml version="1.0" encoding="utf-8" ?>
<Weavers>
  <MobCAT.ClientSecrets/>
</Weavers>
```  

## Usage
Place the **ClientSecret** [Attribute](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/#using-attributes) on a [Field](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/fields) to set it using an environment variable by the same name.  

e.g.  

```
[ClientSecret]
public static readonly string SecretKey = string.Empty;
```

Alternatively, you can specify the environment variable explicitly using the optional *key* parameter.  

e.g.

```
[ClientSecret("THE_SECRET_KEY")]
public static readonly string SecretKey = string.Empty;
```

## Indicative Output
Before build:

```
public static class Config
{
    [ClientSecret]
    public static readonly string SecretKey = string.Empty;
}
```

After build:

```
public static class Config
{
    public static readonly string SecretKey;

    static Config()
    {
        SecretKey = "<SECRET_KEY_VALUE>";
    }
}
```

## Further Considerations

- **Non-String Fields**  
    While it is possible to place the **ClientSecret** attribute on any Field, this has been designed to work with **System.String** types only. Use on types other than **System.String** will result in build failures.  

- **Literal Fields**  
    [Readonly](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/readonly) fields are preferred over [constants](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/const) when using the **ClientSecret** Attribute. References to [constant](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/const) fields, from within the same assembly, would not reflect subsequent changes made to the [IL (Intermediate Language)](https://docs.microsoft.com/en-us/dotnet/standard/managed-code#intermediate-language--execution) binary due to the way the compiler propagates [constant](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/const) fields. References to these fields from a separate assembly on the other hand will see the [constant](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/const) value as it is defined in the updated [IL (Intermediate Language)](https://docs.microsoft.com/en-us/dotnet/standard/managed-code#intermediate-language--execution) binary.