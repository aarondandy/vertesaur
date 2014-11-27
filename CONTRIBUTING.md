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
and as always check to see if a similar issue already exists. All major code changes or functional changes should be related to an issue and should be claimed by somebody when worked.
People can not be assigned to issues using the GitHub assignment feature so instead leave a comment.
This is to prevent multiple people working on the same problem independently and to allow for discussion.
If you are having trouble finding an issue to work on try searching the code for `// TODO` or `NotImplementedException` and create an issue based on those.

## How to Contribute Code

### Legal

For most minor changes it is probably not worth the trouble to deal with legal issues.
Larger contributions and those dealing with algorithms will be of concern however.
The simplest way to keep your contribution in this project is to just license your contributions
under the same license as the project. If you are working on behalf of an organization, hold patents or are committing crazy amounts of code something like a CLA will need to be worked out.
Don't let legal concerns prevent contributions though!
If you have concerns get in touch with a committer and something will be worked out.

### The Process

1. For functional or large changes, make sure to claim an issue.
2. [Fork the project](https://help.github.com/articles/fork-a-repo/).
3. Create a local branch for the work with a short descriptive name.
4. Work on that branch.
5. Run tests.
6. If needed, rebase your branch against upstream.
7. Push and [initiate a pull request](https://help.github.com/articles/using-pull-requests/).

Don't feel disappointed if your PR is not accepted right away, it will get there.
Especially early on, there may be some learning on both sides.

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
* Use `var` unless you need to declare the type explicitly.
* Test methods should use `snake_case` for readability.
* When in doubt feel free to ask.
* Do not attempt to correct floating/fixed values, you can't win!
  * Especially do not compare things with an epsilon value.
* Every data structure and algorithm should have a specialized [`System.Double` (`double`)](http://msdn.microsoft.com/en-us/library/system.double.aspx) implementation.
* On Windows [configure git to us `autocrlf true`](https://help.github.com/articles/dealing-with-line-endings/) to avoid the [wall of pink](http://www.hanselman.com/blog/YoureJustAnotherCarriageReturnLineFeedInTheWall.aspx).
