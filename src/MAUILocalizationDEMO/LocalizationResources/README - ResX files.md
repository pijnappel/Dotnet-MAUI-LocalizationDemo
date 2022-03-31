# Where are the strings stored for localization?

To be able to translate all the texts in your app, it is ideal to store them separated from UI and business code.
In dotnet this is realized using resx files. They store the values as key value pair.



# How should I organize the resx files?

You can organize them to your liking or the requirements of your solution. Create one or more projects for resx files. 
Create as many resx files in your localization project as required. But remember, therw is a guy who has to go through 
all of them and translate every single string. Try to make it easy for him to understand which string belongs to which
part of the app. This makes it easier for him to find the right words in a different language.



# Why are the *.resx files in a separate project?

This allows handling them separated from the source code. Some localization tools support handling assembly files. 
When sending code to other people/companies for translation, you might want to share only the source strings,
not your source code.

But attention: For the resx to be accessible from other projects/assemblies, it is required to switch the resx file
               from internal to public. You can do this in the upper area (toolbar) of the regx editor.



# How should I name my resource keys

Again, this is up to you. Try to make it clear where it belongs. Keep the name short and not too different 
from the text value in the source language (Englisch?!). I like to use a prefix for categroization 
(MainPage_HelloWorld - "Hello World!"). Avoid spaces in your localization keys. The resx file is
trandlated to code and the keys will be modified to be accessible in "code behind".



# Should I re-use the same key/string in multiple places?

That depends. It's recommended to re-use the usual suspects like 'OK', 'Cancel', 'Apply', ...
I use the Strings_General.resx for those.

For other scenarios I usually do not re-use the keys. There is always the possibility that a change 
causes unintentional changes in another view I am not aware of.


# What about wildcards?

When creating messages that contain values, you can use e.g. the string.Format() syntax to include wildcards.
The syntax must be communicated to the translator. Do not use many different approaches for this. It will lead to
confusion and translation errors. (MainPage_CurrentCount - "Current Count: {0}" >> "Current Count: 42")


# After translation?

Once the resx files are translated, the resx files will use the same name plus a culture code plus ".resx".
After copying these files (Strings_General.de.resx, String_General.fr.resx, ...) into the same location as the original
(Strings_General.resx), they are ready to be used. Your app will now automatically choose the translated string, if the 
culture is set to the corresponding language.

var desiredCulture = new CultureInfo("de");
Thread.CurrentThread.CurrentCulture = desiredCulture;
Thread.CurrentThread.CurrentUICulture = desiredCulture;

// To have every new thread use the culture, also set:

CultureInfo.DefaultThreadCurrentCulture = desiredCulture;
CultureInfo.DefaultThreadCurrentUICulture = desiredCulture;
