# MongoDB FrameworkSerializer
[![Build status](https://ci.appveyor.com/api/projects/status/s9sk5u3aa0plc3o1/branch/master?svg=true)](https://ci.appveyor.com/project/glucaci/mongodb-frameworkserializer/branch/master)
[![Tests](https://img.shields.io/appveyor/tests/glucaci/mongodb-frameworkserializer/master.svg)](https://ci.appveyor.com/project/glucaci/mongodb-frameworkserializer)

MongoDB support for using ISerializable interface from .NET

By using the ISerializable interface the object will describe itself how it should be serialized. It's only tide to the .NET serialization contract and dose not brake it's encapsulation in order to be saved.

The object will not be dependent on any persistency library and can be designed free of any constrains. The data layer implementation can be changed any time with something else that support the ISerializable interface.

### Support
- [x] Primitive type
- [x] Object
- [x] Collections
- [ ] Interface
- [ ] Abstract class
- [ ] Cycle dependency

# Getting Started

1. [Defining a Serializable Object](#defining-a-serializable-object)
2. [Register FrameworkSerializer](#register-frameworkserializer)
3. [Use MongoDB Collection](#use-mongodb-collection)

- Optional is possible to [define a type alias](#define-type-alias---optional). This will brake the coupling between the concret type name and what is saved in database.

## Defining a Serializable Object
The object must implement [```ISerializable```](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.iserializable?view=netcore-2.1). A correct implementation of [```ISerializable```](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.iserializable?view=netcore-2.1) must define a ```private``` constructor for sealed classes or a ```protected``` constructor for unsealed classes.
```csharp
public sealed class FullName : ISerializable
{
    private readonly string _firstName;
    private readonly string _lastName;

    public FullName(string firstName, string lastName)
    {
        _firstName = firstName;
        _lastName = lastName;
    }

    public static explicit operator string(FullName fullName)
    {
        return $"{fullName._firstName}, {fullName._lastName}";
    }

    private FullName(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
        {
            throw new ArgumentNullException(nameof(info));
        }
        _firstName = info.GetValue<string>(nameof(_firstName));
        _lastName = info.GetValue<string>(nameof(_lastName));
    }

    void ISerializable.GetObjectData(SerializationInfo info,StreamingContext context)
    {
        info.AddValue(nameof(_firstName), _firstName);
        info.AddValue(nameof(_lastName), _lastName);
    }
}
```
## Register FrameworkSerializer
The FrameworkSerializer must be registerd on application startup.
```csharp
BsonSerializer.RegisterSerializationProvider(FrameworkSerializerProvider.Instance);
```
## Use MongoDB Collection
```csharp
MongoClient client = new MongoClient("mongodb://localhost:27017");
IMongoDatabase db = client.GetDatabase("Users");
_collection = db.GetCollection<FullName>("Names");   
```
### Insert
```csharp
FullName fullName = new FullName("Foo", "Bar");
await _collection.InsertOneAsync(person);
```
```
{
    "_id" : ObjectId("5b6085323a59c131abcfd10b"),
    "__typeAlias" : "FullName",
    "_firstName" : "Foo",
    "_lastName" : "Bar"
}
```
### Read
```csharp
FullName fullName = (await _collection.FindAsync(Builders<FullName>.Filter.Empty))
    .FirstOrDefault();
```
## Define Type Alias - optional
```csharp
[SerializableAlias("FullName")]
public sealed class FullName : ISerializable
{
    // Implementation
}
```
