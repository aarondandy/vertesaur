## License & Legal

This project aims to be licensed under the terms of the [Apache License 2.0](https://tldrlegal.com/license/apache-license-2.0-(apache-2.0)). Nobody wants to deal with the legal stuff but it's important. It is hoped that this project can be free of legal drama in the future and free to evolve into what makes sense. To meet that goal you as a contributor **must** *somehow* deal with the legal side of things to protect yourself as well as the project. I hope to soon have an easy electronic [CLA](Vertesaur-Individual-Contributor-License-Agreement) that can meet this goal.

## Style

The general rule is to make your code look like the existing code but here are a few things to make that easier:

* Indent using spaces and keep tabs out of the code
  * C#: 4 space indentation
* Use [standard naming](http://msdn.microsoft.com/en-us/library/ms229002.aspx) practices.
* Vertical code is preferred to horizontal code, try to keep lines under 120 characters.
* Public fields are OK only when the JIT does not inline.
  * As new versions of .NET/Mono are released this can be changed going into the next major release if performance can be proven equal.
* Add or update XML documentation for members. 
  * Please try to document behavior in `<remarks></remarks>` comments.
*  Use `var` unless you have to declare the type explicitly.
*  When in doubt feel free to ask.

## Arithmetic

Please read [Numerical Values in Vertesaur](https://github.com/aarondandy/vertesaur/wiki/Numerical-Values-in-Vertesaur) for detailed guidance and suggestions on arithmetic in Vertesaur. Here are some quick general rules:

* Do not attempt to correct floating/fixed values.
  * Especially do not compare things with an epsilon value.
* Every data structure and algorithm should have a  specialized [`System.Double`](http://msdn.microsoft.com/en-us/library/system.double.aspx) implementation.