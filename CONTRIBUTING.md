# Contributing

Contributions can be made in many ways, including:

* Issues
* Documentation
* Code
* Community Participation

While there are rules with respect to contribution they are flexible.
If you have a problem with them just discuss it with a committer.

## Issues

Most contributions will revolve around GitHub issues.
If you find a bug or see something that could be improved please create an issue
and as always check to see if a similar issue already exists. All major code changes should be related to an issue and should be claimed by somebody when worked.
People can not be assigned to issues using the GitHub assignment feature so instead leave a comment.
If you are having trouble finding an issue to work on try searching the code for `// TODO` or `NotImplementedException` and create an issue based on those.

## How to Contribute Code

### Legal

For most minor changes it is probably not worth the trouble to deal with legal issues.
Larger contributions and those dealing with algorithms will be of concern however.
The simplest way to keep your contribution in this project is to just license your contributions
under the same license as the project. If you are working on behalf of an organization, hold patents or are committing crazy amounts of code something like a CLA will need to be worked out.
Don't let legal concerns prevent contributions though!
If you have concerns get in touch with a committer and something will be worked out.

### Code Conventions and Style

The general rule is to make your code look like the existing code but here are a few things to make that easier:

* Indent using spaces and keep tabs out of the code
  * C#: 4 space indentation
* Use [standard naming](http://msdn.microsoft.com/en-us/library/ms229002.aspx) practices for the public surface.
* Vertical code is preferred to horizontal code, try to keep lines under 120 characters.
* Public fields are OK only when the JIT does not inline.
  * As new JITs are released this policy may be reconsidered.
* Add or update XML documentation for members. 
  * Try to make XML doc comments as meaningful as possible.
  * Please try to document behavior in `/// <remarks></remarks>` comments when appropriate.
*  Use `var` unless you need to declare the type explicitly.
*  Test methods should use `snake_case` for readability.
*  When in doubt feel free to ask.






## License & Legal

This project aims to be licensed under the terms of the [Apache License 2.0](https://tldrlegal.com/license/apache-license-2.0-(apache-2.0)). Nobody wants to deal with the legal stuff but it's important. It is hoped that this project can be free of legal drama in the future and free to evolve into what makes sense. To meet that goal you as a contributor **must** *somehow* deal with the legal side of things to protect yourself as well as the project. I hope to soon have an easy electronic [CLA](Vertesaur-Individual-Contributor-License-Agreement) that can meet this goal.

## Arithmetic

Please read [Numerical Values in Vertesaur](https://github.com/aarondandy/vertesaur/wiki/Numerical-Values-in-Vertesaur) for detailed guidance and suggestions on arithmetic in Vertesaur. Here are some quick general rules:

* Do not attempt to correct floating/fixed values.
  * Especially do not compare things with an epsilon value.
* Every data structure and algorithm should have a  specialized [`System.Double`](http://msdn.microsoft.com/en-us/library/system.double.aspx) implementation.