# LibraryTask

## Set up
To create the database enter docker-compose up.

## Project summary

This project utilizes the Clean Architecture pattern. For simplicity, I chose to combine the Application and Presentation layers.

The Result pattern used in this project enables more convenient and consistent error handling. It avoids pushing exceptions to upper layers and allows for better customization of error responses.

Transactions are used to ensure atomicity and maintain data integrity throughout the application’s operations.

