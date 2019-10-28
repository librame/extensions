# Librame Design Guidelines 设计准则
Updated in 2019-10-24

Mainly follow the NET framework design guidelines, please refer to the [links](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/) for details.  
主要遵循 C#/.NET 框架设计准则，详情参考此[链接](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/)。

----

# [Naming Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines) [命名准则](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/naming-guidelines)

## [Capitalization Conventions](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions) [大小写约定](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/capitalization-conventions)

### Capitalization Rules for Identifiers 标识符的大小写规则

Don't use underscores.  
不要使用下划线。

* PascalCasing 帕斯卡命名法
* camelCasing 驼峰命名法

Rule References 规则参考

| Identifier 标识符  | Casing 大小写  | Example 示例                              |
|------------------  |--------------  |-----------------------------------------  |
| Namespace          | Pascal         | namespace System.Security {}              |
| Type               | Pascal         | public class StreamReader {}              |
| Interface          | Pascal         | public interface IEnumerable {}           |
| Method             | Pascal         | public string ToString();                 |
| Property           | Pascal         | public int Length { get; }                |
| Event              | Pascal         | public event EventHandler Exited;         |
| Field              | Pascal         | public readonly TimeSpan InfiniteTimeout; |
| Enum               | Pascal         | public enum FileMode { Append, ... }      |
| Parameter          | Camel          | public static int ToInt32(string value);  |

### Capitalizing Compound Words and Common Terms 复合词和常用术语的大写

| Pascal 帕斯卡      | Camel 驼峰     | Not 不是                                  |
|------------------  |--------------  |-----------------------------------------  |
| BitFlag            | bitFlag        | ~~Bitflag~~                               |
| Callback           | callback       | ~~CallBack~~                              |
| Canceled           | canceled       | ~~Cancelled~~                             |
| DoNot              | doNot          | ~~Don't~~                                 |
| Email              | email          | ~~EMail~~                                 |
| Endpoint           | endpoint       | ~~EndPoint~~                              |
| FileName           | fileName       | ~~Filename~~                              |
| Gridline           | gridline       | ~~GridLine~~                              |
| Hashtable          | hashtable      | ~~HashTable~~                             |
| Id                 | id             | ~~ID~~                                    |
| Indexes            | indexes        | ~~Indices~~                               |
| LogOff             | logOff         | ~~LogOut~~                                |
| LogOn              | logOn          | ~~LogIn~~                                 |
| Metadata           | metadata       | ~~MetaData, metaData~~                    |
| Multipanel         | multipanel     | ~~MultiPanel~~                            |
| Multiview          | multiview      | ~~MultiView~~                             |
| Namespace          | namespace      | ~~NameSpace~~                             |
| Ok                 | ok             | ~~OK~~                                    |
| Pi                 | pi             | ~~PI~~                                    |
| Placeholder        | placeholder    | ~~PlaceHolder~~                           |
| SignIn             | signIn         | ~~SignOn~~                                |
| SignOut            | signOut        | ~~SignOff~~                               |
| UserName           | userName       | ~~Username~~                              |
| WhiteSpace         | whiteSpace     | ~~Whitespace~~                            |
| Writable           | writable       | ~~Writeable~~                             |

### Case Sensitivity 区分大小写

| Upper 大写         | Lower 小写     | Equals 相等                               |
|------------------  |--------------  |-----------------------------------------  |
| BitFlag            | bitFlag        | False                                     |

## [General Naming Conventions](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/general-naming-conventions) [通用命名约定](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/general-naming-conventions)

### Word Choice 单词选择

**✓ DO** choose easily readable identifier names.  
**✓ 务必** 选择易读的标识符名称。

**✓ DO** favor readability over brevity.  
**✓ 务必** 使可读性优先于简洁性。

**X DO NOT** use underscores, hyphens, or any other nonalphanumeric characters.  
**X 不要** 使用下划线、连字符或任何其他非字母数字字符。

**X DO NOT** use Hungarian notation.  
**X 不要** 使用匈牙利表示法。

**X AVOID** using identifiers that conflict with keywords of widely used programming languages.  
**X 避免** 使用与广泛应用的编程语言关键字冲突的标识符。

### Using Abbreviations and Acronyms 使用缩写和首字母缩写词

**X DO NOT** use abbreviations or contractions as part of identifier names.  
**X 不要** 在标识符名称中使用缩写形式或缩略形式。

For example, use GetWindow rather than GetWin.  
例如，使用 GetWindow 而非 GetWin。

**X DO NOT** use any acronyms that are not widely accepted, and even if they are, only when necessary.  
**X 不要** 使用任何不常用的首字母缩写形式，即使是常用形式，也应只在必要时使用。

### Avoiding Language-Specific Names 避免特定于语言的名称

**✓ DO** use semantically interesting names rather than language-specific keywords for type names.  
**✓ 务必** 使用在语义上有意义的名称而不是特定于语言的关键字作为类型名称。

For example, GetLength is a better name than GetInt.  
例如，GetLength 比 GetInt 更适合用作名称。

**✓ DO** use a generic CLR type name, rather than a language-specific name, in the rare cases when an identifier has no semantic meaning beyond its type.  
**✓ 务必** 在标识符不具有超出其类型以外的语义时（这是极少见的情况），使用泛型 CLR 类型名称，而不是特定于语言的名称。

For example, a method converting to Int64 should be named ToInt64, not ToLong (because Int64 is a CLR name for the C#-specific alias long). The following table presents several base data types using the CLR type names (as well as the corresponding type names for C#, Visual Basic, and C++).  
例如，转换为 Int64 的方法应命名为 ToInt64，而不是 ToLong（因为 Int64 是特定于 C# 的别名 long 的 CLR 名称）。 下表列出了几种使用 CLR 类型名称（以及 C#、Visual Basic 和 C++ 的对应类型名称）的基本数据类型。

| C#         | Visual Basic     | C++                   | CLR                   |
|----------  |----------------  |---------------------  |---------------------  |
| sbyte      | SByte            | char                  | SByte                 |
| byte       | Byte             | unsigned char         | Byte                  |
| short      | Short            | short                 | Int16                 |
| ushort     | UInt16           | unsigned short        | UInt16                |
| int        | Integer          | int                   | Int32                 |
| uint       | UInt32           | unsigned int          | UInt32                |
| long       | Long             | __int64               | Int64                 |
| ulong      | UInt64           | unsigned __int64      | UInt64                |
| float      | Single           | float                 | Single                |
| double     | Double           | double                | Double                |
| bool       | Boolean          | bool                  | Boolean               |
| char       | Char             | wchar_t               | Char                  |
| string     | String           | String                | String                |
| object     | Object           | Object                | Object                |

**✓ DO** use a common name, such as value or item, rather than repeating the type name, in the rare cases when an identifier has no semantic meaning and the type of the parameter is not important.  
**✓ 务必** 在标识符不具有语义含义且参数类型不重要时（这是极少见的情况），使用常用名称，例如 value 或 item，而不是重复使用类型名称。

## [Names of Assemblies and DLLs](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-assemblies-and-dlls) [程序集和 DLL 的名称](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/names-of-assemblies-and-dlls)

**✓ DO** choose names for your assembly DLLs that suggest large chunks of functionality, such as System.Data.  
**✓ 要** 为程序集 DLL 选择能够使人联想到大量功能的名称，例如 System.Data。

Assembly and DLL names don’t have to correspond to namespace names, but it is reasonable to follow the namespace name when naming assemblies. A good rule of thumb is to name the DLL based on the common prefix of the namespaces contained in the assembly. For example, an assembly with two namespaces, MyCompany.MyTechnology.FirstFeature and MyCompany.MyTechnology.SecondFeature, could be called MyCompany.MyTechnology.dll.  
程序集和 DLL 名称不必与命名空间名称相对应，但在命名程序集时应采用命名空间名称。通常是根据程序集中包含的命名空间的公共前缀来命名 DLL。例如，如果程序集的两个命名空间为 MyCompany.MyTechnology.FirstFeature 和 MyCompany.MyTechnology.SecondFeature，则可将其命名为 MyCompany.MyTechnology.dll。

**✓ CONSIDER** naming DLLs according to the following pattern:  
**✓ 考虑** 按照下面的模式命名 Dll：

```c#
<Company>.<Component>.dll
```

where <Component> contains one or more dot-separated clauses. For example:  
其中<Component>包含一个或多个以点号分隔的子句。例如：

```c#
Litware.Controls.dll。
```

## [Names of Namespaces](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-namespaces) [命名空间的名称](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/names-of-namespaces)

**✓ DO** prefix namespace names with a company name to prevent namespaces from different companies from having the same name.  
**✓ 务必** 使用一个公司名作为命名空间的前缀，以防止不同公司的命名空间具有相同的名称。

**✓ DO** use a stable, version-independent product name at the second level of a namespace name.  
**✓ 务必** 在命名空间名称的第二级名称中使用稳定的、与版本无关的产品名称。

**✓ DO** use PascalCasing, and separate namespace components with periods (e.g., Microsoft.Office.PowerPoint). If your brand employs nontraditional casing, you should follow the casing defined by your brand, even if it deviates from normal namespace casing.  
**✓ 务必** 使用 PascalCasing，并使用句点分隔命名空间组件（例如，Microsoft.Office.PowerPoint）。如果品牌使用非传统的大小写形式，则应遵循由品牌定义的大小写，即使与通常的命名空间大小写不符也是如此。

**✓ CONSIDER** using plural namespace names where appropriate.  
**✓ 考虑** 在适当时使用复数形式的命名空间名称。

For example, use System.Collections instead of System.Collection. Brand names and acronyms are exceptions to this rule, however. For example, use System.IO instead of System.IOs.  
例如，使用 System.Collections 而不是 System.Collection。但此规则不适用于品牌名称和首字母缩写词。例如，使用 System.IO 而不是 System.IOs。

**X DO NOT** use organizational hierarchies as the basis for names in namespace hierarchies, because group names within corporations tend to be short-lived. Organize the hierarchy of namespaces around groups of related technologies.  
**X 不要** 使用企业组织的层次结构作为命名空间层次结构中名称的基础，因为公司内的群组名称往往是短期的。应围绕相关技术的群组来组织命名空间的层次结构。 组织周围的相关技术的组的命名空间的层次结构。

**X DO NOT** use the same name for a namespace and a type in that namespace.  
**X 不要** 对命名空间和该命名空间中的类型使用相同的名称。

For example, do not use Debug as a namespace name and then also provide a class named Debug in the same namespace. Several compilers require such types to be fully qualified.  
例如，如果将命名空间命名为 Debug，就不应在该命名空间中提供一个名为 Debug 的类。 某些编译器要求这些类型为完全限定类型。

### Namespaces and Type Name Conflicts 命名空间和类型名称冲突

**X DO NOT** introduce generic type names such as Element, Node, Log, and Message.  
**X 不要** 引入泛型类型名称，如 Element、Node、Log 和 Message 等。

There is a very high probability that doing so will lead to type name conflicts in common scenarios. You should qualify the generic type names (FormElement, XmlNode, EventLog, SoapMessage).  
没有这样做将导致类型名称冲突共同方案很有可能。 一般情况下，这样做很可能会导致类型名称冲突。应限定泛型类型名称（FormElement、XmlNode、EventLog、SoapMessage）。

## [Names of Classes, Structs, and Interfaces](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-classes-structs-and-interfaces) [类、结构和接口的名称](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/names-of-classes-structs-and-interfaces)

**✓ DO** name classes and structs with nouns or noun phrases, using PascalCasing.  
**✓ 务必** 通过使用 PascalCasing，用名词或名词短语命名类和结构。

This distinguishes type names from methods, which are named with verb phrases.  
这将类型名称与使用谓词短语命名的方法区分开来。

**✓ DO** name interfaces with adjective phrases, or occasionally with nouns or noun phrases.  
**✓ 务必** 使用形容词短语命名接口，或偶尔用名词或名词短语命名接口。

Nouns and noun phrases should be used rarely and they might indicate that the type should be an abstract class, and not an interface.  
应少使用名词和名词短语，它们可能会指示类型为抽象类而非接口。

**✓ CONSIDER** ending the name of derived classes with the name of the base class.  
**✓ 考虑** 使用基类的名称作为派生类名称的结尾部分。

This is very readable and explains the relationship clearly. Some examples of this in code are: ArgumentOutOfRangeException, which is a kind of Exception, and SerializableAttribute, which is a kind of Attribute. However, it is important to use reasonable judgment in applying this guideline; for example, the Button class is a kind of Control event, although Control doesn’t appear in its name.  
这样可让名称非常易读，并清楚体现了关系。 代码中的相关例子有：ArgumentOutOfRangeException，这是一种 Exception，还有 SerializableAttribute，这是一种 Attribute。 但是，在应用此准则时，务必应进行合理判断；例如，Button 类是一种 Control 事件，尽管其名称中并未出现 Control。

**✓ DO** prefix interface names with the letter I, to indicate that the type is an interface.  
**✓ 务必** 在接口名称前加上字母 I 作为前缀，以指示该类型是接口。

For example, IComponent (descriptive noun), ICustomAttributeProvider (noun phrase), and IPersistable (adjective) are appropriate interface names. As with other type names, avoid abbreviations.  
例如，IComponent（描述性名词），ICustomAttributeProvider（名词短语）和 IPersistable（形容词）是合适的接口名称。与其他类型名称一样，应避免使用缩略形式。 对于其他类型名称，请避免缩写形式。

**✓ DO** ensure that the names differ only by the "I" prefix on the interface name when you are defining a class–interface pair where the class is a standard implementation of the interface.  
**✓ 务必** 确保在定义类和接口对时，类名称和接口名称的区别仅在于 "I" 前缀，其中类是接口的标准实现。

**X DO NOT** give class names a prefix (e.g., "C").  
**X 不要** 给类名加前缀（例如，"C"）。

### Names of Generic Type Parameters 泛型类型参数的名称

**✓ DO** name generic type parameters with descriptive names unless a single-letter name is completely self-explanatory and a descriptive name would not add value.  
**✓ 务必** 使用描述性名称命名泛型参数，除非单字母名称可完整体现要传达的含义且描述性名称意义不大。

**✓ CONSIDER** using T as the type parameter name for types with one single-letter type parameter.  
**✓ 考虑** 使用 T 作为具有一个单字母类型参数的类型的类型参数名称。

```c#
public int IComparer<T> { ... }  
public delegate bool Predicate<T>(T item);  
public struct Nullable<T> where T:struct { ... }
```

**✓ DO** prefix descriptive type parameter names with T.  
**✓ 务必** 使用 T 作为描述性类型参数名称的前缀。

```c#
public interface ISessionChannel<TSession> where TSession : ISession {  
    TSession Session { get; }  
}
```

**✓ CONSIDER** indicating constraints placed on a type parameter in the name of the parameter.  
**✓ 考虑** 在参数名称中体现出对该类型参数设置的约束。

For example, a parameter constrained to ISession might be called TSession.  
例如，被限制为 ISession 的参数可能名为 TSession。

### Names of Common Types 常见类型的名称

**System.Attribute**

**✓ DO** add the suffix "Attribute" to names of custom attribute classes.  
**✓ 务必** 为自定义属性类的名称添加后缀 "Attribute"。

**System.Delegate**

**✓ DO** add the suffix "EventHandler" to names of delegates that are used in events.  
**✓ 务必** 向事件中所用委托的名称中添加后缀 "EventHandler"。

**✓ DO** add the suffix "Callback" to names of delegates other than those used as event handlers.  
**✓ 务必** 在用作事件处理程序的委托以外的委托名称中添加后缀 "Callback"。

**X DO NOT** add the suffix "Delegate" to a delegate.  
**X 不要** 将后缀 "Delegate" 添加到委托。

**System.EventArgs**

**✓ DO** add the suffix "EventArgs."  
**✓ 务必** 添加后缀 "EventArgs"。

**System.Enum**

**X DO NOT** derive from this class; use the keyword supported by your language instead; for example, in C#, use the enum keyword.  
**X 不要** 从此类派生；而是使用所用语言支持的关键字；例如，在 C# 中，使用关键字 enum。

**X DO NOT** add the suffix "Enum" or "Flag."  
**X 不要** 添加后缀 "Enum" 或 "Flag"。

**System.Exception**

**✓ DO** add the suffix "Exception."  
**✓ 务必** 添加后缀 "Exception"。

**IDictionary, IDictionary<TKey,TValue>**

**✓ DO** add the suffix "Dictionary." Note that IDictionary is a specific type of collection, but this guideline takes precedence over the more general collections guideline that follows.  
**✓ 务必** 添加后缀 "Dictionary"。 请注意，IDictionary 是一种特定类型的集合，但此准则优先于后面更宽泛的集合准则。

**IEnumerable, ICollection, IList, IEnumerable<T>, ICollection<T>, IList<T>**

**✓ DO** add the suffix "Collection."  
**✓ 务必** 添加后缀 "Collection"。

**System.IO.Stream**

**✓ DO** add the suffix "Stream."  
**✓ 务必** 添加后缀 "Stream"。

**IPermission, CodeAccessPermission**

**✓ DO** add the suffix "Permission."  
**✓ 务必** 添加后缀 "Permission"。

### Naming Enumerations 命名枚举

**✓ DO** use a singular type name for an enumeration unless its values are bit fields.  
**✓ 务必** 为枚举使用单数形式的类型名称，除非枚举值是位域。

**✓ DO** use a plural type name for an enumeration with bit fields as values, also called flags enum.  
**✓ 务必** 为值为位域的枚举使用复数形式的类型名称，这类枚举也称为标志枚举。

**X DO NOT** use an "Enum" suffix in enum type names.  
**X 不要** 在枚举类型名称中使用 "Enum" 作为后缀。

**X DO NOT** use "Flag" or "Flags" suffixes in enum type names.  
**X 不要** 在枚举类型名称中使用 "Flag" 或 "Flags" 作为后缀。

**X DO NOT** use a prefix on enumeration value names (e.g., "ad" for ADO enums, "rtf" for rich text enums, etc.).  
**X 不要** 在枚举值名称中使用前缀（例如，对 ADO 枚举使用 "ad”，对富文本枚举使用 "rtf" 等）。

## [Names of Type Members](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-type-members) [类型成员的名称](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/names-of-type-members)

Types are made of members: methods, properties, events, constructors, and fields. The following sections describe guidelines for naming type members.  
类型的成员包括：方法、属性、事件、构造函数和字段。 以下各部分介绍各类型成员的命名准则。

### Names of Methods 方法的名称

Because methods are the means of taking action, the design guidelines require that method names be verbs or verb phrases. Following this guideline also serves to distinguish method names from property and type names, which are noun or adjective phrases.  
由于方法是操作执行的途径，所以设计准则要求方法名称是谓词（即动词或形容词）或谓词短语。 通过遵循这一准则，还可将方法名称与属性和类型名称（即名词或形容词短语）区分开来。

**✓ DO** give methods names that are verbs or verb phrases.  
**✓ 务必** 使用谓词或谓词短语作为方法名称。

```c#
public class String {  
    public int CompareTo(...);  
    public string[] Split(...);  
    public string Trim();  
}
```

### Names of Properties 属性的名称

Unlike other members, properties should be given noun phrase or adjective names. That is because a property refers to data, and the name of the property reflects that. PascalCasing is always used for property names.  
与其他成员不同，属性名称须为名词短语或形容词。 这是因为属性引用数据，且属性名称反映了这一点。 应始终为属性名称使用 PascalCasing。

**✓ DO** name properties using a noun, noun phrase, or adjective.  
**✓ 务必** 使用名词、名词短语或形容词来命名属性。

**X DO NOT** have properties that match the name of "Get" methods as in the following example:  
**X 不要** 设立与 "Get" 方法名称匹配的属性，如下例所示：

```c#
public string TextWriter { get {...} set {...} }
public string GetTextWriter(int value) { ... }
```

This pattern typically indicates that the property should really be a method.  
此模式通常表明该属性实际上应是一种方法。

**✓ DO** name collection properties with a plural phrase describing the items in the collection instead of using a singular phrase followed by "List" or "Collection."  
**✓ 务必** 使用用于描述集合中项目的复数形式短语来命名集合属性，而不是使用后跟 "List" 或 "Collection" 的单数形式短语来命名。

**✓ DO** name Boolean properties with an affirmative phrase (CanSeek instead of CantSeek). Optionally, you can also prefix Boolean properties with "Is," "Can," or "Has," but only where it adds value.  
**✓ 务必** 使用肯定语气的短语（CanSeek 而不是 CantSeek）来命名布尔型属性。 或者，还可以为布尔型属性使用前缀 "Is"、"Can" 或 "Has"，但仅在适用时使用。

**✓ CONSIDER** giving a property the same name as its type.  
**✓ 考虑** 使用属性的类型的名称为属性命名。

For example, the following property correctly gets and sets an enum value named Color, so the property is named Color:  
例如，以下属性正确获取并设置了名为 Color 的枚举值，因此该属性名为 Color：

```c#
public enum Color {...}  
public class Control {  
    public Color Color { get {...} set {...} }  
}
```

### Names of Events 事件的名称

Events always refer to some action, either one that is happening or one that has occurred. Therefore, as with methods, events are named with verbs, and verb tense is used to indicate the time when the event is raised.  
事件始终是指某个操作，这个操作可能正在发生，也可能已经发生。 因此与方法一样，事件用谓词命名，谓词时态用于指示事件引发的时间。

**✓ DO** name events with a verb or a verb phrase.  
**✓ 务必** 使用谓词或谓词短语来命名事件。

Examples include Clicked, Painting, DroppedDown, and so on.  
示例：Clicked、Painting、DroppedDown 等。

**✓ DO** give events names with a concept of before and after, using the present and past tenses.  
**✓ 务必** 通过使用现在时态和过去时态，让事件名称含有时间先后的概念。

For example, a close event that is raised before a window is closed would be called Closing, and one that is raised after the window is closed would be called Closed.  
例如，窗口关闭之前引发的事件称为 Closing，窗口关闭之后引发的事件称为 Closed。

**X DO NOT** use "Before" or "After" prefixes or postfixes to indicate pre- and post-events. Use present and past tenses as just described.  
请勿使用 “Before” 或 “After” 前缀和后缀来指示事件之前或之后。应按前述使用现在时态和过去时态。

**✓ DO** name event handlers (delegates used as types of events) with the "EventHandler" suffix, as shown in the following example:  
请使用 “EventHandler” 后缀来命名事件处理程序（用作事件类型的委托），如以下示例所示：

```c#
public delegate void ClickedEventHandler(object sender, ClickedEventArgs e);
```

**✓ DO** use two parameters named sender and e in event handlers.  
**✓ 务必** 在事件处理程序中使用两个名为 sender 和 e 的参数。

The sender parameter represents the object that raised the event. The sender parameter is typically of type object, even if it is possible to employ a more specific type.  
sender 参数表示引发事件的对象。sender 参数的类型通常是 object，且可能会使用更具体的类型。

**✓ DO** name event argument classes with the "EventArgs" suffix.  
**✓ 务必** 使用“EventArgs”后缀来命名事件参数类。

### Names of Fields 字段的名称

The field-naming guidelines apply to static public and protected fields. Internal and private fields are not covered by guidelines, and public or protected instance fields are not allowed by the member design guidelines.  
字段命名准则适用于静态公共字段和受保护字段。 准则不适用于内部字段和专用字段，成员设计准则不允许使用公共或受保护的实例字段。

**✓ DO** use PascalCasing in field names.  
请在字段名称中使用 PascalCasing。

**✓ DO** name fields using a noun, noun phrase, or adjective.  
请使用名词、名词短语或形容词来命名字段。

**X DO NOT** use a prefix for field names.  
请勿在字段名称中使用前缀。

For example, do not use "g_" or "s_" to indicate static fields.  
例如，不要使用 "g_" 或 "s_" 来指示静态字段。

## [Naming Parameters](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-parameters) [命名参数](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/naming-parameters)

Beyond the obvious reason of readability, it is important to follow the guidelines for parameter names because parameters are displayed in documentation and in the designer when visual design tools provide Intellisense and class browsing functionality.  
除了提升可读性这一明显目的外，遵循参数名称准则的一个重要原因是文档和设计器（可视化设计工具在提供 Intellisense 和类浏览功能时）中会显示参数。

**✓ DO** use camelCasing in parameter names.  
**✓ 务必** 在参数名称中使用 camelCasing。

**✓ DO** use descriptive parameter names.  
**✓ 务必** 使用描述性参数名称。

**✓ CONSIDER** using names based on a parameter’s meaning rather than the parameter’s type.  
**✓ 考虑** 使用基于参数含义而非参数类型的名称。

### Naming Operator Overload Parameters 命名运算符重载参数

**✓ DO** use left and right for binary operator overload parameter names if there is no meaning to the parameters.  
**✓ 务必** 执行 ：如果参数没有意义，请为二元运算符重载参数名称使用 left 和 right。

**✓ DO** use value for unary operator overload parameter names if there is no meaning to the parameters.  
**✓ 务必** 执行 ：如果参数没有意义，请为一元运算符重载参数名称使用 value。

**✓ CONSIDER** meaningful names for operator overload parameters if doing so adds significant value.  
虑为运算符重载参数使用有意义的名称（如果这样做很有用的话）。

**X DO NOT** use abbreviations or numeric indices for operator overload parameter names.  
**X 不要** 在运算符重载参数名称中使用缩写形式或数值索引。

## [Naming Resources](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-resources) [命名资源](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/naming-resources)

Because localizable resources can be referenced via certain objects as if they were properties, the naming guidelines for resources are similar to property guidelines.  
由于可本地化的资源可像属性那样通过某些对象进行引用，因此资源的命名准则与属性准则相似。

**✓ DO** use PascalCasing in resource keys.  
**✓ 务必** 在资源键中使用 PascalCasing。

**✓ DO** provide descriptive rather than short identifiers.  
**✓ 务必** 提供描述性标识符而非简短标识符。

**X DO NOT** use language-specific keywords of the main CLR languages.  
**X 不要** 使用主要 CLR 语言中的语言特定关键字。

**✓ DO** use only alphanumeric characters and underscores in naming resources.  
**✓ 务必** 执行 ：命名资源中只能使用字母数字字符和下划线。

**✓ DO** use the following naming convention for exception message resources.  
**✓ 务必** 为异常消息资源使用以下命名约定。

The resource identifier should be the exception type name plus a short identifier of the exception:  
资源标识符应为异常类型名称加上异常的短标识符：

```c#
ArgumentExceptionIllegalCharacters
ArgumentExceptionInvalidName
ArgumentExceptionFileNameIsMalformed
```

----

# [Type Design Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/type) [类型设计准则](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/type)

**✓ DO** ensure that each type is a well-defined set of related members, not just a random collection of unrelated functionality.  
**✓ 确保** 每个类型中的成员相互关联，而不仅仅是将不相关的功能集合在一起。

## [Choosing Between Class and Struct](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct) [在类和结构之间选择](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/choosing-between-class-and-struct)

* First, allocations and deallocations of value types are in general cheaper than allocations and deallocations of reference types.  
    首先，值类型的分配和释放通常比引用类型的分配和释放开销更低。

* Next, allocations and deallocations of value type arrays are much cheaper than allocations and deallocations of reference type arrays. In addition, in a majority of cases value type arrays exhibit much better locality of reference.  
    其次，值类型数组的分配和释放比引用类型数组的分配和释放开销更低。此外，在大多数情况下，值类型数组具有更好的引用地址。

Otherwise, Value types get boxed when cast to a reference type or one of the interfaces they implement. They get unboxed when cast back to the value type. In contrast, no such boxing occurs as reference types are cast.  
另外，当值类型转换为引用类型或它们实现的接口之一时，该值类型会被装箱。它们在转回值类型时会被拆箱。相反，在转换引用类型时不会发生这样的装箱。

* Next, reference type assignments copy the reference, whereas value type assignments copy the entire value. Therefore, assignments of large reference types are cheaper than assignments of large value types.  
    再次，引用类型的赋值复制引用，而值类型的赋值复制整个值。因此，大型引用类型的赋值比大型值类型的赋值开销更低。

* Finally, reference types are passed by reference, whereas value types are passed by value. Changes to an instance of a reference type affect all references pointing to the instance. Value type instances are copied when they are passed by value. When an instance of a value type is changed, it of course does not affect any of its copies.  
    最后，引用类型通过引用传递，而值类型通过值传递。对引用类型实例的更改会影响指向该实例的所有引用。值类型实例在按值传递时被复制。 当更改值类型的实例时，它当然不会影响其任何副本。

As a rule of thumb, the majority of types in a framework should be classes. There are, however, some situations in which the characteristics of a value type make it more appropriate to use structs.  
一般来说，框架中的大多数类型应该是类。但是，在某些情况下，值类型的特征使得其更适合使用结构。

**✓ CONSIDER** defining a struct instead of a class if instances of the type are small and commonly short-lived or are commonly embedded in other objects.  
**✓ 考虑** 如果类型的实例比较小并且通常生存期较短或者通常嵌入在其他对象中，则定义结构而不是类。

**X AVOID** defining a struct unless the type has all of the following characteristics:  
**X 避免** 定义一个结构，除非该类型具有所有以下特征：

1. It logically represents a single value, similar to primitive types (int, double, etc.).  
    它逻辑上表示单个值，类似于基元类型（int， double，等等）。

2. It has an instance size under 16 bytes.  
    它的实例大小小于 16 字节。

3. It is immutable.  
    它是不可变的。

4. It will not have to be boxed frequently.  
    它不会频繁装箱。

In all other cases, you should define your types as classes.  
在所有其他情况下，应将类型定义为类。

## [Abstract Class Design](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/abstract-class) [抽象类设计](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/abstract-class)

**X DO NOT** define public or protected internal constructors in abstract types.  
**X 请勿** 在抽象类型中定义公共或受保护的内部构造函数。

**✓ DO** define a protected or an internal constructor in abstract classes.  
**✓ 请** 在抽象类中定义受保护的或内部构造函数

**✓ DO** provide at least one concrete type that inherits from each abstract class that you ship.  
**✓ 请** 提供至少一种继承自你交付的每个抽象类的具体类型。

For example, System.IO.FileStream is an implementation of the System.IO.Stream abstract class.  
例如，System.IO.FileStream 是 System.IO.Stream 抽象类的实现。

## [Static Class Design](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/static-class) [静态类设计](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/static-class)

A static class is defined as a class that contains only static members (of course besides the instance members inherited from System.Object and possibly a private constructor). Some languages provide built-in support for static classes. In C# 2.0 and later, when a class is declared to be static, it is sealed, abstract, and no instance members can be overridden or declared.  
静态类的定义为：仅包含静态成员的类 (当然除了继承自System.Object 的实例成员和可能是私有的构造函数)。某些语言提供对静态类的内置支持。在 C# 2.0 及更高版本中，当类声明为静态时，它是密封、抽象的，并且不能覆盖或声明实例成员。

Static classes are a compromise between pure object-oriented design and simplicity. They are commonly used to provide shortcuts to other operations (such as System.IO.File), holders of extension methods, or functionality for which a full object-oriented wrapper is unwarranted (such as System.Environment).  
静态类是纯面向对象设计和简单性之间的妥协。 它们通常用于提供其他操作的快捷方式（例如System.IO.File ），扩展方法的持有者，或完全面向对象的包装器不合适的功能（例如System.Environment ）。

**✓ DO** use static classes sparingly.  
**✓ 务必** 谨慎使用静态类。

Static classes should be used only as supporting classes for the object-oriented core of the framework.  
静态类应仅用作框架的面向对象核心的支持类。

**X DO NOT** treat static classes as a miscellaneous bucket.  
切忌将静态类视为杂项存储桶。

**X DO NOT** declare or override instance members in static classes.  
切忌在静态类中声明或覆盖实例成员。

**✓ DO** declare static classes as sealed, abstract, and add a private instance constructor if your programming language does not have built-in support for static classes.  
**✓ 务必** 将静态类声明为密封，抽象，并添加一个私有实例构造函数，如果您的编程语言没有内置静态类支持的话。

## [Interface Design](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/interface) [接口设计](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/interface)

The CLR does not support multiple inheritance (i.e., CLR classes cannot inherit from more than one base class), but it does allow types to implement one or more interfaces in addition to inheriting from a base class. Therefore, using an interface is the only option in order to provide a common base type.  
CLR 不支持多重继承（即，CLR 类不能从多个基类继承），但它允许类型除了继承基类之外还实现一个或多个接口。因此使用接口是提供公共基类型的唯一选择。

**✓ DO** define an interface if you need some common API to be supported by a set of types that includes value types.  
**✓ 务必** 定义一个接口，如果需要某些通用 API 受到包含值类型的一组类型的支持。

**✓ CONSIDER** defining an interface if you need to support its functionality on types that already inherit from some other type.  
**✓ 考虑** 定义一个接口，如果需要在已继承自其他类型的类型上支持其功能。

**X AVOID** using marker interfaces (interfaces with no members).  
**X 避免** 使用标记接口（不包含任何成员的接口）。

If you need to mark a class as having a specific characteristic (marker), in general, use a custom attribute rather than an interface.  
如果需要将类标记为具有特定特征（标记），通常使用自定义特性而不是接口。

**✓ DO** provide at least one type that is an implementation of an interface.  
**✓ 务必** 至少提供一种作为接口的实现的类型。

For example, List<T> is an implementation of the IList<T> interface.  
例如，List<T> 是 IList<T> 接口的实现。

**✓ DO** provide at least one API that consumes each interface you define (a method taking the interface as a parameter or a property typed as the interface).  
**✓ 务必** 为你定义的每个接口提供至少一个使用它的 API（将接口作为类型化为接口的参数或属性的方法）。

For example, List<T>.Sort consumes the System.Collections.Generic.IComparer<T> interface.  
例如，List<T>.Sort 使用 System.Collections.Generic.IComparer<T> 接口。

**X DO NOT** add members to an interface that has previously shipped.  
切忌将成员添加到之前发布的接口中。

Doing so would break implementations of the interface. You should create a new interface in order to avoid versioning problems.  
这样做会破坏接口的实现。应创建一个新接口以避免版本控制问题。

Except for the situations described in these guidelines, you should, in general, choose classes rather than interfaces in designing managed code reusable libraries.  
除了这些指南中描述的情况之外，一般情况下，应该在设计托管代码的可重用库时选择类而不是接口。

## [Struct Design](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/struct) [结构设计](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/struct)

**X DO NOT** provide a parameterless constructor for a struct.  
**X 不要** 提供一个无参数构造函数的结构。

Following this guideline allows arrays of structs to be created without having to run the constructor on each item of the array. Notice that C# does not allow structs to have parameterless constructors.  
遵循此原则将允许创建结构数组，而无需对数组的每个项运行构造函数。 请注意，C#不允许结构具有无参数构造函数。

**X DO NOT** define mutable value types.  
切忌定义可变值类型。

**✓ DO** ensure that a state where all instance data is set to zero, false, or null (as appropriate) is valid.  
**✓ 务必** 确保所有实例数据设置为零、false 或 null（视情况而定）的状态是有效的。

This prevents accidental creation of invalid instances when an array of the structs is created.  
这可以防止在创建结构数组时意外创建无效实例。

**✓ DO** implement IEquatable<T> on value types.  
**✓ 务必** 为值类型实现 IEquatable<T>。

The Object.Equals method on value types causes boxing, and its default implementation is not very efficient, because it uses reflection. Equals can have much better performance and can be implemented so that it will not cause boxing.  
值类型的 Object.Equals 方法会导致装箱，并且其默认实现效率不高，因为它使用反射。 Equals 可以有更好的性能，并且可以进行实现，这样它就不会导致装箱。

**X DO NOT** explicitly extend ValueType. In fact, most languages prevent this.  
请勿显式扩展 ValueType。 实际上，大多数语言禁止此行为。

In general, structs can be very useful but should only be used for small, single, immutable values that will not be boxed frequently.  
一般情况下，结构可能非常有用，但应仅用于不经常装箱的单个不可变的小值。

## [Enum Design](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/enum) [枚举设计](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/enum)

Enums are a special kind of value type. There are two kinds of enums: simple enums and flag enums.  
枚举是一种特殊的值类型。枚举分为两种类型：简单枚举和标志枚举。

Simple enums represent small closed sets of choices. A common example of the simple enum is a set of colors.  
简单枚举表示小型的封闭选项集。简单枚举的一个常见示例是一组颜色。

Flag enums are designed to support bitwise operations on the enum values. A common example of the flags enum is a list of options.  
标志枚举旨在支持枚举值的按位运算。标志枚举的一个常见示例是选项列表。

**✓ DO** use an enum to strongly type parameters, properties, and return values that represent sets of values.  
**✓ 务必** 将枚举用于强类型参数、属性和表示一组值集的返回值。

**✓ DO** favor using an enum instead of static constants.  
**✓ 务必** 首选使用枚举而不是静态常量。

**X DO NOT** use an enum for open sets (such as the operating system version, names of your friends, etc.).  
**X 切忌** 将枚举用于开放集（如操作系统版本、朋友的姓名等）。

**X DO NOT** provide reserved enum values that are intended for future use.  
**X 切忌** 提供供将来使用的保留枚举值。

**X AVOID** publicly exposing enums with only one value.  
**X 避免** 公开暴露只有一个值的枚举。

**X DO NOT** include sentinel values in enums.  
**X 切忌** 在枚举中包括 sentinel 值。

**✓ DO** provide a value of zero on simple enums.  
**✓ 务必** 为简单枚举提供零值。

Consider calling the value something like "None." If such a value is not appropriate for this particular enum, the most common default value for the enum should be assigned the underlying value of zero.  
考虑将值称为“None”等类似名称。如果此类值不适合此特定枚举，则应为该枚举的最常见默认值指定为零的基础值。

**✓ CONSIDER** using Int32 (the default in most programming languages) as the underlying type of an enum unless any of the following is true:  
**✓ 考虑** 使用 Int32（大多数编程语言中的默认值）作为枚举的基础类型，除非满足以下任何条件：

1. The enum is a flags enum and you have more than 32 flags, or expect to have more in the future.  
   枚举是一个标志枚举，包含 32 个以上标志，或者将来可能有更多标志。

2. The underlying type needs to be different than Int32 for easier interoperability with unmanaged code expecting different-size enums.  
   在枚举可能大小各不相同的情况下，基础类型需要与 Int32 不同，以便更容易地与非托管代码进行互操作。

3. A smaller underlying type would result in substantial savings in space. If you expect the enum to be used mainly as an argument for flow of control, the size makes little difference. The size savings might be significant if:  
   较小的基础类型将大大节省空间。如果希望将枚举主要用作控制流的参数，则大小差别不大。如果符合以下条件，则可以大大节省空间：

    * You expect the enum to be used as a field in a very frequently instantiated structure or class.  
        希望将枚举在非常频繁实例化的结构或类中用作字段。

    * You expect users to create large arrays or collections of the enum instances.  
        希望用户创建枚举实例的大型数组或集合。

    * You expect a large number of instances of the enum to be serialized.  
        希望序列化大量枚举实例。

**✓ DO** name flag enums with plural nouns or noun phrases and simple enums with singular nouns or noun phrases.  
**✓ 务必** 为标志枚举使用复数名词或名词短语命名，为简单枚举使用单数名词或名词短语命名。

**X DO NOT** extend System.Enum directly.  
**X 切忌** 直接扩展 System.Enum。

System.Enum is a special type used by the CLR to create user-defined enumerations. Most programming languages provide a programming element that gives you access to this functionality. For example, in C# the enum keyword is used to define an enumeration.  
System.Enum 是 CLR 用于创建用户定义的枚举的特殊类型。大多数编程语言都提供了一个编程元素，来使你可以使用此功能。例如，在 C# 中，enum 关键字用于定义枚举。

### Designing Flag Enums 设计标志枚举

**✓ DO** apply the System.FlagsAttribute to flag enums. Do not apply this attribute to simple enums.  
**✓ 务必** 对标志枚举应用 System.FlagsAttribute。不要将此特性应用于简单枚举。

**✓ DO** use powers of two for the flag enum values so they can be freely combined using the bitwise OR operation.  
**✓ 务必** 对标志枚举值使用 2 的幂，以便可以使用按位 OR 运算自由组合它们。

**✓ CONSIDER** providing special enum values for commonly used combinations of flags.  
**✓ 考虑** 为常用的标志组合提供特殊的枚举值。

Bitwise operations are an advanced concept and should not be required for simple tasks. ReadWrite is an example of such a special value.  
按位运算是一种高级概念，简单任务应无需使用。ReadWrite 就是这种特殊值的一个例子。

**X AVOID** creating flag enums where certain combinations of values are invalid.  
**X 避免** 创建某些值组合无效的标志枚举。

**X AVOID** using flag enum values of zero unless the value represents "all flags are cleared" and is named appropriately, as prescribed by the next guideline.  
**X 避免** 使用值为零的标志枚举，除非该值表示“已清除所有标志”，并按照下一指南的规定进行适当命名。

**✓ DO** name the zero value of flag enums None. For a flag enum, the value must always mean "all flags are cleared."  
**✓ 务必** 将值为零的标志枚举命名为 None。于标志枚举，该值必须始终表示“已清除所有标志”。

### Adding Value to Enums 将值添加到枚举

It is very common to discover that you need to add values to an enum after you have already shipped it. There is a potential application compatibility problem when the newly added value is returned from an existing API, because poorly written applications might not handle the new value correctly.  
你常常会发现，需要在已发布枚举后向其添加一个值。 从现有 API 返回新添加的值时，会存在潜在的应用程序兼容性问题，因为编写不当的应用程序可能无法正确处理新值。

**✓ CONSIDER** adding values to enums, despite a small compatibility risk.  
**✓ 考虑** 将值添加到枚举，监管存在较低的兼容性风险。

If you have real data about application incompatibilities caused by additions to an enum, consider adding a new API that returns the new and old values, and deprecate the old API, which should continue returning just the old values. This will ensure that your existing applications remain compatible.  
如果已获得了有关向枚举添加值而引起的应用程序不兼容性问题的实际数据，请考虑添加可返回新值和旧值的新 API，并弃用旧 API，该旧 API 应会继续仅返回旧值。这将确保现有的应用程序保持兼容。

## [Nested Types](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/nested-types) [嵌套类型](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/nested-types)

**✓ DO** use nested types when the relationship between the nested type and its outer type is such that member-accessibility semantics are desirable.  
**✓ 务必** 使用嵌套的类型，从而使成员可访问性语义都需要的嵌套的类型和其外部类型之间的关系时。

**X DO NOT** use public nested types as a logical grouping construct; use namespaces for this.  
**X 切忌** 使用公共嵌套的类型作为自己的逻辑分组构造; 为此使用命名空间。

**X AVOID** publicly exposed nested types. The only exception to this is if variables of the nested type need to be declared only in rare scenarios such as subclassing or other advanced customization scenarios.  
**X 避免** 公共公开嵌套的类型。唯一的例外是嵌套类型的变量需要声明仅在极少数情况下，如生成子类或其他高级自定义方案。

**X DO NOT** use nested types if the type is likely to be referenced outside of the containing type.  
**X 切忌** 使用嵌套的类型，如果类型为可能包含类型的外部引用。

For example, an enum passed to a method defined on a class should not be defined as a nested type in the class.  
例如，传递给方法的类上定义的枚举应未定义为类中的嵌套类型。

**X DO NOT** use nested types if they need to be instantiated by client code. If a type has a public constructor, it should probably not be nested.  
**X 切忌** 使用嵌套的类型，如果他们需要通过客户端代码来实例化。如果类型具有公共构造函数，它可能不应进行嵌套。

**X DO NOT** define a nested type as a member of an interface. Many languages do not support such a construct.  
**X 切忌** 定义为接口成员的嵌套的类型。许多语言不支持此类构造。

----

# [Member Design Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/member) [成员设计准则](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/member)

## [Member Overloading](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/member-overloading) [成员重载](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/member-overloading)

Member overloading means creating two or more members on the same type that differ only in the number or type of parameters but have the same name. For example, in the following, the WriteLine method is overloaded:  
成员重载意味着在同一类型上创建两个或多个成员，这些成员仅在参数的数量或类型中不同，但具有相同的名称。 例如，在下面的中，将重载 WriteLine 方法：

```c#
public static class Console {  
    public void WriteLine();  
    public void WriteLine(string value);  
    public void WriteLine(bool value);  
    ...  
}
```

Because only methods, constructors, and indexed properties can have parameters, only those members can be overloaded.  
由于只有方法、构造函数和索引属性可以有参数，因此只能重载这些成员。

**✓ DO** try to use descriptive parameter names to indicate the default used by shorter overloads.  
**✓ 务必** 尝试使用描述性的参数名称以指示较短的重载使用的默认值。

**X AVOID** arbitrarily varying parameter names in overloads. If a parameter in one overload represents the same input as a parameter in another overload, the parameters should have the same name.  
**X 避免** 随意更改重载中的参数名称。如果一个重载中的参数表示与另一个重载中的参数相同的输入，则这些参数应具有相同的名称。

**X AVOID** being inconsistent in the ordering of parameters in overloaded members. Parameters with the same name should appear in the same position in all overloads.  
**X 避免** 在排序中的参数中不一致状态重载成员。具有相同名称的参数应出现在所有重载的同一位置。

**✓ DO** make only the longest overload virtual (if extensibility is required). Shorter overloads should simply call through to a longer overload.  
**✓ 务必** 使只有最长的重载成为虚方法（如果可扩展性是必需的）。更短的重载只需调用更长的重载。

**X DO NOT** use ref or out modifiers to overload members.  
**X 切忌** 使用ref或out修饰符来重载成员。

**X DO NOT** have overloads with parameters at the same position and similar types yet with different semantics.  
**X 切忌** 具有重载随着在相同的位置以及相似类型的参数，而使用不同的语义。

**✓ DO** allow null to be passed for optional arguments.  
**✓ 务必** 允许为可选参数传递 null 值。

~~**✓ DO** use member overloading rather than defining members with default arguments.~~  
~~**✓ 务必** 使用成员重载，而不是使用默认参数定义成员。~~

~~Default arguments are not CLS compliant.~~  
~~默认参数不符合 CLS。~~

**✓** It is recommended to define members with default parameters first, which reduces member overloading.  
**✓** 推荐优先使用默认参数定义成员，这样可以减少成员重载。

## [Property Design](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/property) [属性设计](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/property)

**✓ DO** create get-only properties if the caller should not be able to change the value of the property.  
**✓ 务必** 创建 get-only 属性，如果调用方不能更改属性值的话。

Keep in mind that if the type of the property is a mutable reference type, the property value can be changed even if the property is get-only.  
请记住，如果属性的类型是可变的引用类型，则即使属性为 get-only，也可以更改属性值。

**X DO NOT** provide set-only properties or properties with the setter having broader accessibility than the getter.  
**X 切忌** 提供 set-only 属性，或其 setter 的可访问性比 getter 更广泛的属性。

For example, do not use properties with a public setter and a protected getter.  
例如，不要使用具有公共 setter 和受保护的 getter 的属性。

If the property getter cannot be provided, implement the functionality as a method instead. Consider starting the method name with Set and follow with what you would have named the property. For example, AppDomain has a method called SetCachePath instead of having a set-only property called CachePath.  
如果无法提供属性 getter，请将该功能实现为方法。考虑将方法名称以 Set 开始，在后面添加你对该属性的命名。如，AppDomain 有一个名为 SetCachePath 的方法，而不是一个名为 CachePath 的 set-only 属性。

**✓ DO** provide sensible default values for all properties, ensuring that the defaults do not result in a security hole or terribly inefficient code.  
**✓ 务必** 为所有属性提供合理的默认值，确保默认值不会导致安全漏洞或严重低效的代码。

**✓ DO** allow properties to be set in any order even if this results in a temporary invalid state of the object.  
**✓ 务必** 允许将属性按任何顺序设置，即使这会导致对象的临时无效状态。

**✓ DO** preserve the previous value if a property setter throws an exception.  
**✓ 务必** 在属性 setter 引发异常时保留以前的值。

**X AVOID** throwing exceptions from property getters.  
**X 避免** 从属性 getter 引发异常。

Property getters should be simple operations and should not have any preconditions. If a getter can throw an exception, it should probably be redesigned to be a method. Notice that this rule does not apply to indexers, where we do expect exceptions as a result of validating the arguments.  
属性 getter 应该是简单的操作，不应该有任何前置条件。如果某个 getter 可能会引发异常，则应该将其重新设计为方法。 请注意，此规则不适用于索引器，因为我们确实会因验证参数而导致异常。

### Indexed Property Design 索引属性设计

An indexed property is a special property that can have parameters and can be called with special syntax similar to array indexing.  
索引属性是一个特殊属性，可以具有参数，并且可以使用与数组索引类似的特殊语法进行调用。

Indexed properties are commonly referred to as indexers. Indexers should be used only in APIs that provide access to items in a logical collection. For example, a string is a collection of characters, and the indexer on System.String was added to access its characters.  
索引属性通常称为索引器。 索引器应仅用于提供对逻辑集合中项目的访问的 API。 例如，字符串是字符的集合，在索引器上添加了 System.String 即可访问其字符。

**✓ CONSIDER** using indexers to provide access to data stored in an internal array.  
**✓ 考虑** 使用索引器以提供对存储在内部数组中的数据访问。

**✓ CONSIDER** providing indexers on types representing collections of items.  
**✓ 考虑** 提供索引器上表示的项的集合的类型。

**X AVOID** using indexed properties with more than one parameter.  
**X 避免** 使用索引与多个参数的属性。

If the design requires multiple parameters, reconsider whether the property really represents an accessor to a logical collection. If it does not, use methods instead. Consider starting the method name with Get or Set.  
如果设计需要多个参数，请重新考虑该属性是否真正代表逻辑集合的访问者。 如果不是，请改用方法。考虑使用 Get 或 Set 开头的方法名称。

**X AVOID** indexers with parameter types other than System.Int32, System.Int64, System.String, System.Object, or an enum.  
**X 避免** 具有以外的其他参数类型的索引器 System.Int32， System.Int64， System.String， System.Object，或枚举。

If the design requires other types of parameters, strongly reevaluate whether the API really represents an accessor to a logical collection. If it does not, use a method. Consider starting the method name with Get or Set.  
如果设计需要其他类型的参数，请仔细重新评估 API 是否真正代表逻辑集合的访问者。如果不是，请使用方法。考虑使用 Get 或 Set 开头的方法名称。

**✓ DO** use the name Item for indexed properties unless there is an obviously better name (e.g., see the Chars[Int32] property on System.String).  
**✓ 务必** 使用名称 Item 的索引属性，除非有明显更好的名称（例如，请参阅 Chars[Range] 属性 System.String）。

In C#, indexers are by default named Item. The IndexerNameAttribute can be used to customize this name.  
在 C# 中，索引器默认名称为 Item。IndexerNameAttribute 可用于自定义此名称。

**X DO NOT** provide both an indexer and methods that are semantically equivalent.  
**X 切忌** 提供索引器和在语义上等效的方法。

**X DO NOT** provide more than one family of overloaded indexers in one type.  
**X 切忌** 提供多个系列中一种类型的重载索引器。

This is enforced by the C# compiler.  
此准则由 C# 编译器强制执行。

**X DO NOT** use nondefault indexed properties.  
**X 切忌** 使用非默认索引属性。

This is enforced by the C# compiler.  
此准则由 C# 编译器强制执行。

### Property Change Notification Events 属性更改通知事件

**✓ CONSIDER** raising change notification events when property values in high-level APIs (usually designer components) are modified.  
**✓ 考虑** 在修改高级API（通常是设计器组件）中的属性值时引发更改知事件。

If there is a good scenario for a user to know when a property of an object is changing, the object should raise a change notification event for the property.  
如果具备可让用户知道对象的属性何时发生变化的有效方案，则该对象应该该属性引发更改通知事件。

However, it is unlikely to be worth the overhead to raise such events for low-level APIs such as base types or collections. For example, List<T> would not raise such events when a new item is added to the list and the Count property changes.  
但是，可能并不值得为基础类型或集合等低级 API 引发此类事件。 例如，向列表添加新项且 Count 属性更改时，List<T> 不会引发此类事件。

**✓ CONSIDER** raising change notification events when the value of a property changes via external forces.  
**✓ 考虑** 当属性值因外力而变化时，引发更改通知事件。

If a property value changes via some external force (in a way other than by calling methods on the object), raise events indicate to the developer that the value is changing and has changed. A good example is the Text property of a text box control. When the user types text in a TextBox, the property value automatically changes.  
如果属性值通过某种外力（通过调用对象上的方法以外的方式）发生更改。则引发事件向开发人员指示值正在更改并已更改。 一个典型示例是文本框控件的 Text 属性。 当用户在 TextBox 中键入文本时，属性值会自动更改。

## [Constructor Design](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/constructor) [构造函数设计](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/constructor)
...

## [Event Design](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/event) [事件设计](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/event)
...

## [Field Design](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/field) [字段设计](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/field)
...

## [Extension Methods](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/extension-methods) [扩展方法](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/extension-methods)
...

## [Operator Overloads](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/operator-overloads) [运算符重载](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/operator-overloads)
...

## [Parameter Design](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/parameter-design) [参数设计](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/parameter-design)
...

----

# [Designing for Extensibility](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/designing-for-extensibility) [扩展性设计](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/designing-for-extensibility)

## [Unsealed Classes](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/unsealed-classes) [未密封类](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/unsealed-classes)
...

## [Protected Members](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/protected-members) [受保护的成员](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/protected-members)
...

## [Events and Callbacks](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/events-and-callbacks) [事件和回调](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/events-and-callbacks)
...

## [Virtual Members](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/virtual-members) [虚成员](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/virtual-members)
...

## [Abstracts (Abstract Types and Interfaces)](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/abstractions-abstract-types-and-interfaces) [抽象（抽象类型和接口）](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/abstractions-abstract-types-and-interfaces)
...

## [Base Classes for Implementing Abstractions](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/base-classes-for-implementing-abstractions) [用于实现抽象的基类](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/base-classes-for-implementing-abstractions)
...

## [Sealing](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/sealing) [密封](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/sealing)
...

----

# [Design Guidelines for Exceptions](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/exceptions) [异常性设计准则](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/exceptions)

## [Exception Throwing](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/exception-throwing) [异常引发](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/exception-throwing)
...

## [Using Standard Exception Types](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/using-standard-exception-types) [使用标准异常类型](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/using-standard-exception-types)
...

## [Exceptions and Performance](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/exceptions-and-performance) [异常和性能](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/exceptions-and-performance)
...

----

# [Usage Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/usage-guidelines) [使用准则](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/usage-guidelines)

## [Arrays](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/arrays) [数组](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/arrays)
...

## [Attributes](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/attributes) [特性](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/attributes)
...

## [Collections](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/guidelines-for-collections) [集合](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/guidelines-for-collections)
...

## [Serialization](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/serialization) [序列化](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/serialization)
...

## [System.Xml Usage](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/system-xml-usage) [使用情况](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/system-xml-usage)
...

## [Equality Operators](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/equality-operators) [相等运算符](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/equality-operators)
...

----

# [Common Design Patterns](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/common-design-patterns) [常见设计模式](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/common-design-patterns)

## [Dependency Properties](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/dependency-properties) [依赖项属性](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/dependency-properties)
...
