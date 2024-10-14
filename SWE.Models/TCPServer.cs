using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Globalization;


namespace SWE.Models
{
    public class TCPServer
    {
        private int _port;
        private TcpListener _listener;

        private static List<User> user = new();

        private static List<Package> packages = new();

        private static int userAmount = 0;


        public TCPServer(int port)
        {
            _port = port;
        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            Console.WriteLine($"Server running on port {_port}");

            while (true)
            {

                TcpClient client = _listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                HandleRequest(stream);

                client.Close();
            }
        }

        private void HandleRequest(NetworkStream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string request = reader.ReadLine();
            if (string.IsNullOrEmpty(request)) return;

            string[] tokens = request.Split(' ');
            if (tokens.Length < 2) return;

            string method = tokens[0];
            string url = tokens[1];

            string replaced = "";

            Dictionary<string, string> headers = new Dictionary<string, string>();
            while (!string.IsNullOrWhiteSpace(request = reader.ReadLine()))
            {
                string[] headerParts = request.Split(": ", 2, StringSplitOptions.None);
                if (headerParts.Length == 2)
                {
                    headers[headerParts[0]] = headerParts[1];
                }
            }

            string body = null;
            if (headers.ContainsKey("Content-Length"))
            {
                int contentLength = int.Parse(headers["Content-Length"]);
                char[] buffer = new char[contentLength];
                reader.Read(buffer, 0, contentLength);
                body = new string(buffer);
            }

            if (url.Contains("/users") && url != "/users")
            {

                string bearerToken = string.Empty;

                if (headers.ContainsKey("Authorization"))
                {
                    bearerToken = headers["Authorization"].Replace("Bearer ", "").Trim();
                }

                string tempURL = url;
                replaced = tempURL.Replace("/users", "").Replace("/", "");

                User knownUser = user.FirstOrDefault(l => l.setGetToken == bearerToken);

                if (knownUser != null)
                {
                    url = "/users/controll";
                }
            }

            int methodHASH = method.GetHashCode();
            int urlHASH = url.GetHashCode();

            switch (methodHASH)
            {
                case int i when i == "GET".GetHashCode():
                    switch (urlHASH)
                    {
                        case int n when n == "/users/controll".GetHashCode():
                            HandleUserControllGET(stream, body, headers, replaced);
                            break;
                        case int n when n == "/cards".GetHashCode():
                            HandleStackCards(stream, body, headers);
                            break;
                        case int n when n == "/deck".GetHashCode():
                            HandleDeckCards(stream, body, headers);
                            break;
                        case int n when n == "/deck?format=plain".GetHashCode():
                            HandleDeckCards(stream, body, headers);
                            break;
                        default:
                            SendResponse(stream, "404 Not Found", "Route not found");
                            break;
                    }
                    break;
                case int i when i == "POST".GetHashCode():
                    switch (urlHASH)
                    {
                        case int n when n == "/users".GetHashCode():
                            HandleRegisterUser(stream, body);
                            break;
                        case int n when n == "/sessions".GetHashCode():
                            HandleLogin(stream, body);
                            break;
                        case int n when n == "/packages".GetHashCode():
                            HandlePackages(stream, body, headers);
                            break;
                        case int n when n == "/transactions/packages".GetHashCode():
                            HandleTransPackages(stream, body, headers);
                            break;
                        default:
                            SendResponse(stream, "404 Not Found", "Route not found");
                            break;
                    }
                    break;
                case int i when i == "PUT".GetHashCode():
                    switch (urlHASH)
                    {
                        case int n when n == "/deck".GetHashCode():
                            HandlePutDeck(stream, body, headers);
                            break;
                        case int n when n == "/users/controll".GetHashCode():
                            HandleUserControllPUT(stream, body, headers, replaced);
                            break;
                        default:
                            SendResponse(stream, "404 Not Found", "Route not found");
                            break;
                    }
                    break;
                default:
                    SendResponse(stream, "404 Not Found", "Route not found");
                    break;

            }
        }

        private Dictionary<string, string> TestBody(string body)
        {

            Dictionary<string, string> data = new Dictionary<string, string>();


            if (!string.IsNullOrEmpty(body))
            {

                body = body.Trim('{', '}');
                string[] pairs = body.Split(',');

                foreach (string pair in pairs)
                {
                    string[] keyValue = pair.Split(':');
                    string key = keyValue[0].Trim(' ', '"');
                    string value = keyValue[1].Trim(' ', '"');
                    data[key] = value;
                }

                return data;
            }
            return data;
        }

        private User isAuthentic(Dictionary<string, string> headers)
        {
            string bearerToken = string.Empty;

            if (headers.ContainsKey("Authorization"))
            {
                bearerToken = headers["Authorization"].Replace("Bearer ", "").Trim();

                User userExists = user.FirstOrDefault(j => j.setGetToken == bearerToken);

                if (userExists != null)
                {
                    return userExists;
                }
            }
            return null;
        }

        private void HandleUserControllPUT(NetworkStream stream, string body, Dictionary<string, string> headers, string userName)
        {

            Dictionary<string, string> data = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(body))
            {

                body = body.Trim('{', '}');
                string[] pairs = body.Split(',');

                foreach (string pair in pairs)
                {
                    string key = "";
                    string value = "";

                    if (!pair.Contains("Image"))
                    {
                        string[] keyValue = pair.Split(':');
                        key = keyValue[0].Trim(' ', '"');
                        value = keyValue[1].Trim(' ', '"');
                    }
                    else
                    {
                        key = "Image";
                        string tempStringPair = pair.Replace(key, "");
                        tempStringPair = tempStringPair.Remove(0, 6);
                        tempStringPair = tempStringPair.Trim(' ', '"');
                        value = tempStringPair;

                    }

                    data[key] = value;
                }

            }

            User userExists = isAuthentic(headers);

            if (userExists != null)
            {
                if (userExists.setGetToken == (userName + "-mtcgToken"))
                {
                    userExists.changeAccount(data);

                    Console.WriteLine("Changed Account Info for " + userExists.Username);

                    SendResponse(stream, "200", "");
                    return;

                }
            }

            Console.WriteLine("Unautherized");
            SendResponse(stream, "409", "Unautherized");

        }

        private void HandleUserControllGET(NetworkStream stream, string body, Dictionary<string, string> headers, string userName)
        {


            Dictionary<string, string> data = TestBody(body);

            User userExists = isAuthentic(headers);

            if (userExists != null)
            {
                if (userExists.setGetToken == (userName + "-mtcgToken"))
                {

                    userExists.printUserData();
                    SendResponse(stream, "200", "user data");
                    return;
                }
            }

            Console.WriteLine("Unautherized");
            SendResponse(stream, "409", "Unautherized");

        }

        private void HandlePutDeck(NetworkStream stream, string body, Dictionary<string, string> headers)
        {
            List<string> data = new List<string>();

            if (!string.IsNullOrEmpty(body))
            {

                body = body.Trim('[', ']');
                string[] pairs = body.Split(',');

                foreach (string pair in pairs)
                {
                    string key = pair.Trim(' ', '"');
                    data.Add(key);
                }
            }
            else
            {
                Console.WriteLine("Empty Body");
                SendResponse(stream, "409", "Empty Body");
            }

            if (data.Count() != 4)
            {
                Console.WriteLine("Please enter 4");
                SendResponse(stream, "409", "Enter 4 Cards");
                return;
            }

            
            User userExists = isAuthentic(headers);

            if (userExists != null)
            {
                foreach (string id in data)
                    {

                        Cards cardExists = userExists.getSetStack.FirstOrDefault(j => j != null && j.GetSetID == id);

                        if (cardExists != null)
                        {
                            if (!userExists.setGetDeck.Contains(cardExists))
                            {
                                userExists.setGetDeck.Add(cardExists);
                                Console.WriteLine("Card added to deck");

                            }
                        }
                        else
                        {
                            Console.WriteLine("You do not own this card");
                            SendResponse(stream, "409", "Not owned Card");
                            return;
                        }
                    }
                    SendResponse(stream, "201", "");
                    return;
            }

            Console.WriteLine("Unautherized");
            SendResponse(stream, "409", "Unautherized");

        }

        private void HandleDeckCards(NetworkStream stream, string body, Dictionary<string, string> headers)
        {
            Dictionary<string, string> data = TestBody(body);

            User userExists = isAuthentic(headers);

            if (userExists != null)
            {
                userExists.printDeck();
                SendResponse(stream, "200", "list of cards in Deck");
                return;
            }

            Console.WriteLine("Unautherized");
            SendResponse(stream, "409", "Unautherized");
        }

        private void HandleStackCards(NetworkStream stream, string body, Dictionary<string, string> headers)
        {
            Dictionary<string, string> data = TestBody(body);


            User userExists = isAuthentic(headers);

            if (userExists != null)
            {
                userExists.printStack();
                SendResponse(stream, "200", "list of cards in Stack");
                return;
            }

            Console.WriteLine("Unautherized");
            SendResponse(stream, "409", "Unautherized");

        }

        private void HandleTransPackages(NetworkStream stream, string body, Dictionary<string, string> headers)
        {
            Dictionary<string, string> data = TestBody(body);


            User userExists = isAuthentic(headers);

            if (userExists != null)
            {
                if (userExists.setGetCoins >= 4)
                {
                    int i = packages.Count();

                    if (i <= 0)
                    {
                        Console.WriteLine(">>>> NO MORE PACKAGES REMAIN <<<<\n");
                        SendResponse(stream, "409", "No Packages available");
                    }
                    else
                    {
                        userExists.setGetCoins = userExists.setGetCoins - 4;
                        userExists.addToStack(packages[0].card);
                        Console.WriteLine(userExists.Username + " bought Package. Packages remain: " + i);
                        packages.RemoveAt(0);
                        SendResponse(stream, "201", "");
                    }
                }
                else
                {
                    if (userExists.setGetCoins < 4)
                    {
                        Console.WriteLine(">>>> NOT ENOUGH MONEY <<<<\n");
                        SendResponse(stream, "409", "Not enough money");
                    }
                }
            }
        }

        private void HandlePackages(NetworkStream stream, string body, Dictionary<string, string> headers)
        {
            Dictionary<string, string> data = TestBody(body);

            if (headers.ContainsKey("Authorization"))
            {
                if (headers["Authorization"] == String.Concat("Bearer ", "admin-mtcgToken"))
                {

                    var packageData = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(body);

                    Package pack = new Package();

                    int i = 0;

                    foreach (var creature in packageData)
                    {
                        pack.card[i] = new Cards { GetSetID = creature["Id"].ToString(), GetSetCardName = creature["Name"].ToString(), GetSetDamage = (float.Parse(creature["Damage"].ToString(), CultureInfo.InvariantCulture)) };
                        i++;
                    }

                    packages.Add(pack);

                    Console.WriteLine("---- Admin added Packages to buy ---- (insgesamt: " + packages.Count() + " )");

                    SendResponse(stream, "201", "");
                }
                else
                {
                    SendResponse(stream, "409", "Not Autharized User");
                }
            }
        }

        private void HandleLogin(NetworkStream stream, string body)
        {
            if (string.IsNullOrEmpty(body)) return;

            Dictionary<string, string> data = new Dictionary<string, string>();

            body = body.Trim('{', '}');
            string[] pairs = body.Split(',');

            foreach (string pair in pairs)
            {
                string[] keyValue = pair.Split(':');
                string key = keyValue[0].Trim(' ', '"');
                string value = keyValue[1].Trim(' ', '"');
                data[key] = value;
            }

            // Werte extrahieren und verarbeiten
            if (data.ContainsKey("Username") && data.ContainsKey("Password"))
            {
                string username = data["Username"];
                string password = data["Password"];

                if (userAmount == 0)
                {
                    SendResponse(stream, "409", "No Users  Exists");
                    Console.WriteLine("No Users Exists");
                }
                else
                {
                    User userExists = user.FirstOrDefault(t => t.Username == username);

                    if (userExists == null)
                    {
                        SendResponse(stream, "409", "No User with given Username exists");
                        Console.WriteLine("No User with given Username exists");
                    }
                    else
                    {
                        if (userExists.Username == username && userExists.setGetPassword == password)
                        {
                            if (userExists.setGetToken == string.Empty)
                            {
                                userExists.setGetToken = String.Concat(username, "-mtcgToken");
                                SendResponse(stream, "200", userExists.setGetToken);
                                Console.WriteLine("Usertoken created " + userExists.setGetToken);
                            }
                        }
                        else
                        {
                            SendResponse(stream, "409", "Login Failed");
                            Console.WriteLine("Login Failed: Wrong Password");
                        }
                    }
                }
            }
        }

        private void HandleRegisterUser(NetworkStream stream, string body)
        {
            Dictionary<string, string> data = TestBody(body);


            if (data.ContainsKey("Username") && data.ContainsKey("Password"))
            {
                string username = data["Username"];
                string password = data["Password"];

                if (userAmount == 0)
                {
                    user.Add(new User { setGetUserID = userAmount, Username = username, setGetPassword = password });
                    userAmount++;
                    SendResponse(stream, "201", "User Added");
                    Console.WriteLine("User added");
                }
                else
                {
                    User userExists = user.FirstOrDefault(t => t.Username == username);

                    if (userExists != null)
                    {
                        SendResponse(stream, "409", "User already exists");
                        Console.WriteLine("User already exists");
                    }
                    else
                    {
                        user.Add(new User { setGetUserID = userAmount, Username = username, setGetPassword = password });
                        userAmount++;
                        SendResponse(stream, "201", "User Added");
                        Console.WriteLine("User added");
                    }
                }
            }
        }

        private void SendResponse(NetworkStream stream, string status, string body)
        {
            string response = $"HTTP/1.1 {status}\r\n" +
                              "Content-Type: text/plain\r\n" +
                              $"Content-Length: {body.Length}\r\n" +
                              "\r\n" +
                              body;

            byte[] buffer = Encoding.UTF8.GetBytes(response);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}