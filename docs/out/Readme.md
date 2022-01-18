# Illusion Script
By Christoph Koschel and the HexaStudio Fundation

## Content

- [What is Illusion Script](#About)
- [Basic usage](#BasicUsage)
- [Data Types](#DataTypes)
    - [Number](#Numbers)
    - [String](#String)
    - [List](#String)
    - [Object](#Object)
- [Syntax](#Syntax)
    - [Basic operation](#BasicOperation)
    - [Identifier](#Identifier)
    - [Variable declaration](#VariableDeclaration)
    - [Functions](#Functions)
    - [Loops ](#Loops)
    - [Headers](#Headers)
- [Modules](#Modules)
    - [std](#std)
    - [os](#os)
    - [fs](#fs)
    - [math](#math)
- [Errors](#Errors)
    - [Invalid Syntax Error](#ISE)
    - [Runtime Error](#RTE)
- [Future of Illusion Script](#FututeOfIllusionScript)


## What is Illusion Script <span id="About"></span>

Illusion Script is a open source lightweight programming language


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


## Syntax <span id="Syntax"></span>

### Basic operation <span id="BasicOperation"></span>

Use simple or complicated math operations easily in the ils-shell or directly through a ils file

```
5 + 5
```

| Usage | Code | Example | Result | 
|--|--|--|--| 
| Plus | + | 5 + 5 | 10 | 
| Minus | - | 5 - 5 | 0 | 
| Multiply| * | 5 * 5 | 25 | 
| Divided| / | 5 / 5 | 1 | 
| Power by | ^ | 5 ^ 5 | 3125 | 
| Modulo | % | 15 % 6 | 3 |


### Identifier <span id="Identifier"></span>

Identifiers are needed for the declaration of variables and functions, they can contain letters from a to z, 0 - 9 and
underscores. But a declaration cannot start with a number

```
let x = 20;
```


### Variable declaration <span id="VariableDeclaration"></span>

**Declaration**
Use the keyword `let` with a identifier to declare a variable

```
let x = 20;
```

Use the special syntax to declare multiple variables with the same value

```
let x = let y = 20;
```

Enter in the shell now `x` and `y` and you can see the output is both `20`

**Scoping**
Illusion Script creates a new variable context on function calls and files import

```
@import "std";
let a = 20;
if true then
	let a = 40;
	print(a);
	print(endl);
el;

print(a);
```

The result in the console should be

```
40
40
```

The result is different for functions

```
@import "std";
let a = 20;
define change()
	let a = 40;
	print(a);
	print(endl);
el;

change();
print(a);
```

```
40
20
```

**Re-Declaring**
To change the value of a variable just use the declaration syntax from above

```
let a = 20;
print(a);
print(endl);
let a = 40;
print(a);
```

```
20
40
```



### Functions <span id="Functions"></span>

**Declaration and Calling**
Use the keyword `define` and a identifier to declare a function there are two kinds of functions. The one line and the
multi-line function.

```
@import "std";

define function1() => print(5 + 5);

define function2()
	print(5+5);
	print(endl);
	print(2+2);
el
```

Functions can be easily called

```
function1();
print(endl);
function2();
```

And the result should be

```
10
10
4
```

**Parameters**
The usage of parameters is important for functions. To declare a parameter write a identifier between the brackets in
the function constructor by multiple parameters set a comma between the identifier.

```
@import "std";

define function1(x)
	print(x + 2);
	print(endl)
el

function1(2);
function1(4);
function1(6);
function1(8);
```

```
4
6
8
10
```

**Anonymous, information and coping**
Illusion Script supports anonymous functions. They can used in parameters or in variables

```
@import "std";

let func1 = define() => print(5 + 5);

define baseFunction(x)
	x();
el;

baseFunction(define() => print(10 / 2));
print(endl);
func1();

```

```
10
5
```

To get information about a function just write the identifier without the brackets into the code

```
print(func1);
print(endl);
print(baseFunction);
```

```
<function <anonymous>[main.ils (9)]>
<function baseFunction[main.ils (11)]>
```

Also you can copy a function

```
@import "std";

define a()
	print(5);
	print(endl);
el;

let b = a;
a();
b();
```

```
5
5
```

**Return**
To exit the function early or return a result use the `ret` keyword

```
@import "std";

define a(x)
	if x == 4 then
		ret x;
	el;
	
	ret x * 2;
el;

print(a(5));
print(endl);
print(a(4));
```

```
10
4
```


## Loops <span id="Loops"></span>

The syntax of the while loop is simple use the keyword `while` with a expression behind


## Headers <span id="Headers"></span>

In many examples above you see the line `@import "std";`. This is a header statement. They can only stand at the top of
a file. All header statements has a `@` at the beginning.
\
\
With the `@import` statement modules or files can be included. Illusion Script is a flexible language. So you can make
flexible imports
\
\
windows.ils

```
@import "std";

exp define greet(username)
	print("Hello " + username + endl);
	print("Your operating system is Windows");
el;
```

\
linux.ils

```
@import "std";

exp define greet(username)
	print("Hello " + username + endl);
	print("Your operating system is Linux");
el;
```

\
main.ils

```
@import "os";
@import "std";
@if OS_NAME == "win32" then
	@import "./windows.ils";
@else
	@import "./linux.ils";
@el

greet("Christoph");
```

```
Hello Christoph
Your operating system is Windows
```


