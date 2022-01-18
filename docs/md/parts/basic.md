## Basic usage <span id="BasicUsage"></span>

To use the ils language there are two possibilities by simply executing the ils executable a shell opens where you can
directly execute ils commands or you enter a file as argument which can be used for the execution.
**Shell**

```
Microsoft Windows [Version 10.0.19043.1165]
(c) Microsoft Corporation. Alle Rechte vorbehalten.

C:\Users\Christoph>ils.exe
>5 + 5

10

>
```

**File**

main.ils

```
5 + 5
```

```
Microsoft Windows [Version 10.0.19043.1165]
(c) Microsoft Corporation. Alle Rechte vorbehalten.

C:\Users\Christop>ils.exe -f=./main.ils

10

C:\Users\Christoph>
```
