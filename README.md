## net-cdgen
This is my code generator working with C# webapis

  - This is based on my current needs.
  - I will continue to update as long as my needs evolve.
  - Currently under construction, it might fail.

### Installation

This tool generate code based on [.NET Core](https://nodejs.org/) v3+ .

```sh
$ dotnet tool install --global netcdgen --version 1.1.0
```

#### Documentation
```sh


commands:

netcdgen controller
    Options:
         -m  | --model ModelName               You must specify your model name ex: User.
         -nm | --namespace DirectoryNamespace  You must specify your projects namespace ex: myWebApi.
netcdgen model 
    Options: 
        -m  | --model ModelName               You must specify your model name ex: User.
        -nm | --namespace DirectoryNamespace  You must specify your projects namespace ex: myWebApi.
        -a  | --attributes [type:name,...]    You must specify your model attributes in an array where type = [int,string,bool,...] and name = yourpropertyname.
        -gts | --genToStr => [OPTIONAL]       NotImplemented: If you want to generate a toStringMethod
netcdgen service
    Options: 
        -m  | --model     ModelName           You must specify your model name ex: User.
        -nm | --namespace DirectoryNamespace  You must specify your projects namespace ex: myWebApi.
  -mongo | -sql                         OnlyMongoImplemented: You must specify type of service.
  
```


License
----

MIT

