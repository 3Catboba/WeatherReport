# WeatherReport

## Motivation/ Background

Current weather report mobile applications are heavily bundles with various functionalities that are filled with membership, advertisements, or payments. They take up a lot more than needed storage space and memory in our device. In this project, in order to practice and examine my ability of developing mobile applications, I decide to develop a light weight and user friendly online mobile applications to fill the need of simple and original weather report application.

## APP Explanation

### Features
  My APP includes a number of features. 
    1. Sign Up: 
        The application uses a online database to store the key information of a user and let users to register an account to keep track of their information
    2. Third Party Authorization: 
        Besides conventional signing up, the APP also provides an alternative to sign in through a third party account such as google.
    3. Location Based Weather: 
        Upon logging in to the weather page, the system will automatically track the user's location and provide local weather information by default
    4. Free Search Weather: 
        The user can also enter any text in the search text box and an online API will perform fuzzy search and give back valid result
    5. Account Setting: 
        The user is allowed to set a default city to begin with instead of its local position. 
    6. Custom Styling: 
        Users are also able to change the background color by their mood. 
    7. Account Deletion: 
        If users decide to not use the APP anymore, the APP also offers a feature to delete all their informations stored in the database to ensure maximum privacy.

### User Interfacing and Styling
    [LINK]


### Version Control Summary:
    2 branches, 1 commit, 1 merge

### MVVM Integration
  The login page is implemented using MVVM deisgn.

### Software Design Patterns: 
  Four Patterns are used in the project, each one is explained with example of code provided. 
  1. **Singleton Pattern**
    This pattern ensures that a class has only one instance and provides a global point of access to it. In your project, the MongoDBHelper is effectively used as a singleton, though not explicitly implemented as one.
    example
  ```
  public static DatabaseHelper Database { get; private set; }
  public App(Auth0Client auth0Client, IMongoDatabase database)
  {
      InitializeComponent();
      Database = new DatabaseHelper(database);
      _auth0Client = auth0Client;
  
      MainPage = new NavigationPage(new LoginPage(_auth0Client, Database));
  }
  ```
  2. **Repository Pattern**
  The MongoDBHelper class has the logic for accessing the MongoDB database, effectively acting as a repository. This pattern abstracts the data layer, making it easier to manage and test.
  ```
  public class MongoDBHelper
  {
      private readonly IMongoDatabase _database;
  
      public MongoDBHelper(IMongoDatabase database)
      {
          _database = database;
      }
  
      public IMongoCollection<UserRecord> GetUsersCollection()
      {
          return _database.GetCollection<UserRecord>("Users");
      }
  
      public async Task<int> SaveUserAsync(UserRecord user)
      {
          var collection = GetUsersCollection();
          await collection.InsertOneAsync(user);
          return 1;
      }
  
      public async Task<UserRecord> GetUserByEmailAsync(string email)
      {
          var collection = GetUsersCollection();
          var filter = Builders<UserRecord>.Filter.Eq("Email", email);
          var user = await collection.Find(filter).FirstOrDefaultAsync();
          return user;
      }
  
      public async Task UpdateUserAsync(UserRecord user)
      {
          var collection = GetUsersCollection();
          var filter = Builders<UserRecord>.Filter.Eq("Email", user.Email);
          var update = Builders<UserRecord>.Update
              .Set(u => u.Username, user.Username)
              .Set(u => u.DefaultCity, user.DefaultCity)
              .Set(u => u.BackgroundColor, user.BackgroundColor);
          await collection.UpdateOneAsync(filter, update);
      }
  
      public async Task DeleteUserAsync(string email)
      {
          var collection = GetUsersCollection();
          var filter = Builders<UserRecord>.Filter.Eq("Email", email);
          await collection.DeleteOneAsync(filter);
      }
  }
  ```
  3. **Dependency Injection**
  In the App class, Auth0Client and MongoDBHelper are injected into the constructors of other classes like LoginPage and WeatherPage
  ```
  public App(Auth0Client auth0Client, IMongoDatabase database)
  {
      InitializeComponent();
      Database = new DatabaseHelper(database);
      _auth0Client = auth0Client;
  
      MainPage = new NavigationPage(new LoginPage(_auth0Client, Database));
  }
  ```
  4. **MVVM**
  Like mentioned above, Model: Represents the data (e.g., UserRecord). View: Represents the UI (e.g., LoginPage.xaml).ViewModel: Contains the logic and data-binding
  ```
  public partial class LoginPage : ContentPage
  {
      public LoginPage(Auth0Client auth0Client, MongoDBHelper mongoDBHelper)
      {
          InitializeComponent();
          BindingContext = new LoginViewModel(auth0Client, mongoDBHelper);
      }
  }
  ```

  ### Seperation of Concerns
  All code and files are organized in different folders based on its functionality and purpose.

  ### Database Integration
  The application integrates with the online MongoDB Atlas database, four CRUD opertions are included:
    **Create**: SaveUserAsync
    **Read**: GetUserByEmailAsync
    **Update**: UpdateUserAsync
    **Delete**: DeleteUserAsync


  ### Cloud Integration
    Three external online sources are used
    
    **MongoDB** : Online API to access the database 4 endpoints(Create/Read/Update/Delete)
    **Auth0** : online third party authentication-authorisation 1 endpoint
    **OpenWeather** : online API to retrieve realtime weather information 1 endpoint

    
    
