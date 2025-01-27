# Project Setup Instructions

## Forking the Repository

1. Navigate to [CCI.Selenium.Technical Repo](https://github.com/CCI-SDET-Technical/CCI.Selenium.Technical).
2. Click the "Fork" button at the top right of the repository page.
3. Select your GitHub account to fork the repository.

## Cloning Your Fork

1. Go to your GitHub profile and find the forked repository.
2. Click the "Code" button and copy the URL.
3. Open your terminal or command prompt.
4. Run the following command to clone your forked repository:
    ```sh
    git clone https://github.com/your-username/your-forked-repo.git
    ```
    Replace `your-username` with your GitHub username and `your-forked-repo` with the name of the repository.

## Setting Up the Project

1. Navigate to the project directory:
    ```sh
    cd your-forked-repo
    ```
2. Open the project in your preferred IDE (e.g., Visual Studio, Visual Studio Code).

## Running the Project

1. Ensure you have the .NET 8.0 SDK installed. You can download it from [here](https://dotnet.microsoft.com/download).
2. In the terminal, navigate to the project directory and run:
    ```sh
    dotnet run
    ```

## Writing Your Tests

1. Create a new branch for your feature or bug fix:
    ```sh
    git checkout -b feature-branch-name
    ```
2. Make your changes and commit them:
    ```sh
    git add .
    git commit -m "Description of your changes"
    ```
3. Push your changes to your forked repository:
    ```sh
    git push origin feature-branch-name
    ```
4. Share your forked repository url with there hiring manager

Thank you for contributing!