# Addmusic2 (tenative title)

## Intro

Addmusic2 (tenative title) is a C#.net rewrite of KungFuFurby's [AddmusicKFF fork](https://github.com/KungFuFurby/AddMusicKFF) and aims to provide an improved user experience to the current Addmusic version. The rewritten code is based on AddmusicKFF v1.0.11. 

## Goals

There are several goals:
- Improve upon the existing c++ implementation
- Create an updated parser that allows for easier enhancements and support for additional grammars, variations, and features
- Add support for Localiztions other than English for international users
- Create a new GUI to replace the existing one
    - Support management of settings and without having to edit them by hand
- PATH support for Windows and similar cmd execution support for OSX and Linux
- Feature parity with the current version of AddmusicKFF

## Enhancements

There are several enhancements to the current Addmusic:
- Improved Parser for Song MML and SFX MML files
- Support for custom Localization of various messages
- Converts the existing `Addmusic_list.txt`, `Addmusic_sample groups.txt`, and `Addmusic_sound effects.txt` to new Json files

## Commandline Parameters

| Name | Command | Description |
| --- | --- | --- |
| ROM Name | [--r, --rom] | The Name of the ROM to modify. |
| Convert | [--c] | Force off conversion from Addmusic 4.05 and AddmusicM |
| Check Echo | [--e] | Turn off echo buffer checking. |
| Bank Start | [--b] |Do not attempt to save music data in bank 0x40 and above. |
| Verbose Logging | [--v, --verbose] | Turn verbosity on.  More information will be displayed while using this.|
| Aggresive Free Space | [--a] | Make free space finding more aggressive. |
| Duplicate Checking | [--d] | Turn off duplicate sample checking. |
| Hex Validation | [--h] | Turn off hex command validation. |
| Create Patch | [--p] | Create a patch, but do not patch it to the ROM. |
| Optimize Sample Usage | [--u] | Turn off Optimize Sample Usage. |
| Allow SA1 | [--s] | Turn off allowing SA1 enabled features. |
| Dump Sound Effects | [--dumpsfx, --sfxdump] | Dump sound effects |
| Visualize | [--visualize] | Generates a visualization of the SPC. |
| Name | [--srd, --streamredirect] | Description |
| Generate SPC | [--norom] | Only generate SPC files, no ROM required. |
| Help | [--?, --help] | Lists and shows help information for the various commands. |