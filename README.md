# ClausaComm
## a Skype-like LAN chatting app

![ab](https://user-images.githubusercontent.com/57546404/153777989-38f5bb60-687f-4936-88ec-42428c8f02c2.jpg)

## How to install
Just download the [installer](https://github.com/AspireOne/ClausaComm-Installer) and it will be installed as a regular program. Optionally you can download just the [binaries](https://github.com/AspireOne/ClausaComm/releases).

Updates are installed automatically.

## What is ClausaComm?
ClausaComm is a free LAN chatting app. You can connect to other computers in the same network and communicate with them. The idea and functionality is similar to any other chatting app, like Skype (or more advanced Discord), except that this one is LAN-based.

## Why?
This project initially started when I saw a hole in the market of LAN chatting and calling programs. The main idea was that with LAN voice/video calling, there would be practically no delay, and that comes in handy in fast-paced multiplayer games. I was also curious as to how networking (the whole chatting/calling/connecting infrastructure) is solved in chatting programs. And so I set myself to the challenge of creating one from scratch.

That was the first impulse that got me going, but from that point on I just did it as a hobby and to gain experience. So it's mainly a proof of concept :)

My guess was that it would take me just about two weeks, but not too soon it became clear that I was terribly wrong, and that there will be far more complexity involved than I initially thought. A year later and I can finally proudly say that I did it, and I did it well.

## Features
- Profiles (name, profile picture, status...) updated in real-time
- Compatibility for offline contacts (data is properly synced) and dynamic IPs
- FAST Sending of files (archives, photos, files, folders...) with UNLIMITED size (50gb? no problem)
- Messaging (chat history, delivery status, sounds and notifications, send error handling...)
- Exportable chatlog and contact list
- Automatic updates
- All the other basic functionality a chatting app should have.

## How to clone / contribute
Clone the repo, download nuget dependencies, install the appropriate .NET SDK (if needed) and you are set. To contribute, create an appropriate branch for the changes you are going to make, and when you are done, create a pull request.

## Rant
Choosing Windows Forms for a new and such a large and complex project was an insane idea. Everything must be done from scratch there (every millimeter of every control and it's functionality), things don't work the way they should (e.g. layouts), componenets are buggy (e.g. the notorious flickering on repaints), the designer is unusable at times... Ridiculously basic things wasted me an unbelievable amount of time.

Don't get me wrong, Windows Forms is definitely nice in a lot of ways; but it's not ideal for larger and mainly new projects. There are newer and better things out there.
