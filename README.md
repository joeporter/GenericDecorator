GenericDecorator
================

Illustrative example of a GME decorator in C#, based on code by Tihamer Levendovzsky, Sandor Nyako, and others. Thanks to Kevin Smyth for answering my questions about the decorator interface and function.

This project builds against GME version 14.3.5 or later using Visual Studio 2010 on Windows 7.  It should also work with later versions of Windows and Visual Studio, but as of this check-in it has not been tested.  Visual C# Express 2010 should also work fine.  That needs to be tested soon.

GME is available here:
https://forge.isis.vanderbilt.edu/gme/

Visual Studio 2010 Express can be found here (for now):
http://www.visualstudio.com/downloads/download-visual-studio-vs#DownloadFamilies_4

To try out the example, clone the repository and follow these steps:

1. Register the paradigm file in GME (GenericDecorator/meta/SignLanguage.xme).
2. Open Visual Studio in Administrator mode (right-click the Visual Studio entry in the start menu and select it).
3. Open the solution file (GenericDecorator/GenericDecorator.sln) and compile the code.
4. Open the example model (GenericDecorator/meta/SignLanguageTest.xme).

Documentation can be found in GenericDecorator/doc/GenericDecorator.pdf.

Please contact me (Joe Porter) at jporter@isis.vanderbilt.edu if you have problems or questions.
