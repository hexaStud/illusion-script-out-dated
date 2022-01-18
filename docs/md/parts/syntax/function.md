
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
