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
  Four Patterns are used in the project: 
  1. Singleton Pattern
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

    
