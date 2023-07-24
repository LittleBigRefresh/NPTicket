# NPTicket
An open-source C# library for reading/verifying PSN authentication tickets.

This library is mostly based off of Project Lighthouse's implementation.
We've taken some creative liberties and shipped it as a package for use by other PS3 custom server developers.

## Projects using this package
Here's a list of projects using NPTicket for reference.
You can also look at the `NPTicket.Test` project in this repository for a basic overview.

- [Refresh](https://github.com/LittleBigRefresh/Refresh)
- [SoundShapesServer](https://github.com/turecross321/SoundShapesServer)
- [PLGarage](https://github.com/jackcaver/PLGarage)

## Useful Documentation and Repositories

- [PSDevWiki/X-I-5-Ticket](https://psdevwiki.com/ps3/X-I-5-Ticket):
    General overview of how tickets function, as well as examples

- [ProjectLighthouse's Documentation](https://github.com/LBPUnion/ProjectLighthouse/blob/main/Documentation/Tickets.md):
    Documents NPTicket's encryption.

- [Skateboard3Server's Documentation](https://github.com/hallofmeat/Skateboard3Server/blob/master/docs/PS3Ticket.md):
    Documents format in more in-depth detail.

- [PubKeyFinder](https://github.com/Slendy/PubKeyFinder):
    Small C# program that implements the ECDSA public key recovery algorithm.