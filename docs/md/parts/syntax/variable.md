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
