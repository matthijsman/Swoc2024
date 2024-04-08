To get the visualisation going, you'll need [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity)

To set it up: 
1. Open Package Manager window (Window | Package Manager)
1. Click `+` button on the upper-left of a window, and select "Add package from git URL..."
1. Enter the following URL and click `Add` button

```
https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity
```

Next, install [YetAnotherHttpHandler](https://github.com/Cysharp/YetAnotherHttpHandler), again using the "Add package from git URL..." option

```
https://github.com/Cysharp/YetAnotherHttpHandler.git?path=src/YetAnotherHttpHandler
```

After a restart, the nuget option should be available in unity, and all needed packages installed when building 
