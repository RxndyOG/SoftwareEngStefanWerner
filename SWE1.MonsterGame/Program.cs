using SWE.Models;
using System;

//HttpServer.Start("http://localhost:10001/");

// Erstelle eine Instanz des Servers auf Port 10001
TCPServer server = new TCPServer(10001);

// Starte den Server
server.Start();



