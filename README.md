# BTFindTree
此项目为 Leo 项目中的高效查找数据结构，使用二分+桶的方式优化查找性能。

# 使用场景

为了动态构建高性能查找‘字典’所提供的算法，可以结合 Natasha 创建出无锁高并发的键值对查找功能。
性能好于并发字典，在高频访问场景下比较有用。

# 具体算法

- Hash 二分查找树

  该算法将使用字典中的 Key 作为查找依据，通过 roslyn Release模式优化成二三查找树，提供高性能的查找方法。
  
- 模糊指针查找树

  该算法针对 Key 类型为字符串的场景，使用者传入字符串后算法将自动寻找特征点，没用的字符会被跳过，因为仅匹配特征，所以性能超高。
  但满足特征的字串都可执行 value, 该算法的使用场景通常是作者经过深思熟虑后的。
  
 - 归并最小权查找树

    该算法也称为精确指针查找树，通过分治处理每个字符串和匹配次数，然后归并结果，计算最小权值，相比模糊指针查找树，
  该算法是精确的，它虽然利用了特征，但是没有进行跳跃处理，该匹配的都要匹配到才能拿到结果。  
  



# 使用方法

使用前提：

  了解并发字典的使用方法。

- 使用自定义二分查找树  

```C#  

var dict = Dictionary<T,string>();
dict["a"] = "return 1;";
dict["abc"] = "return 2;";  

var script = BTFTemplate.GetCustomerBTFScript(dict,"arg.GetHashCode()",item=>item.GetHashCode().ToString())+"return default;";  
以上便构建了一段完整的利用HashCode查找的方法体。
//如switch(arg.GetHashCode()){ case 28273847: xxx }
//其中case 28273847 是由 GetCustomerBTFScript 第三个参数，Func<TKey,string> 委托得到的。

```  

- 使用 Hash 二分查找树

```C# 
  
    //Key :   可以为任意类型，因为真正用到T的是它的HashCode
    //value:  比如是字符串; return 1;/ Action(a); / a=1; 等正常代码字符串。
    //        作用是当用户传入 key 的时候执行 value.
    
    var dict = Dictionary<T,string>();
    dict["a"] = "return 1;";
    dict["abc"] = "return 2;";
    string result = BTFTemplate.GetHashBTFScript( dict );
    
    //拿到 result 使用 natasha 构造。
    //例如：HashDelegate = NDomain.Random().UnsafeFunc<string, int>(BTFTemplate.GetHashBTFScript(ScriptDict) + "return default;");
    
 ```
 
 - 使用模糊指针查找树
 
 
```C# 
  
    //Key :   必须是字串，因为模糊树和最小权都是针对字串的一种算法结构
    //value:  比如是字符串; return 1;/ Action(a); / a=1; 等正常代码字符串。
    //        作用是当用户传入 key 的时候执行 value.
    
    var dict = Dictionary<string,string>();
    dict["a"] = "return 1;";
    dict["abc"] = "return 2;";
    string result = BTFTemplate.GetFuzzyPointBTFScript( dict );
    
    //拿到 result 使用 natasha 构造。
    //例如：HashDelegate = NDomain.Random().UnsafeFunc<string, int>(BTFTemplate.GetFuzzyPointBTFScript(ScriptDict) + "return default;");
    
 ```
 
  - 使用归并最小权查找树
 
 
```C# 
  
    //Key :   必须是字串，因为模糊树和最小权都是针对字串的一种算法结构
    //value:  比如是字符串; return 1;/ Action(a); / a=1; 等正常代码字符串。
    //        作用是当用户传入 key 的时候执行 value.
    
    var dict = Dictionary<string,string>();
    dict["a"] = "return 1;";
    dict["abc"] = "return 2;";
    string result = BTFTemplate.GetGroupPrecisionPointBTFScript( dict );
    
    //拿到 result 使用 natasha 构造。
    //例如：HashDelegate = NDomain.Random().UnsafeFunc<string, int>(BTFTemplate.GetPrecisionPointBTFScript(ScriptDict) + "return default;");
    
 ```  
 
 
 
