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
