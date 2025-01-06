-- Step 1: Create the database
CREATE DATABASE MessageDB;
GO

-- Step 2: Use the newly created database
USE MessageDB;
GO

-- Step 3: Create the Messages table
CREATE TABLE Messages (
    MessageId INT IDENTITY(1,1) PRIMARY KEY,       -- Primary key with auto-increment
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(), -- Date/Time the message was created
    Recipient NVARCHAR(255) NOT NULL,             -- Recipient of the message
    Message NVARCHAR(MAX) NOT NULL                -- The message content
);

-- Step 4: Create the MessageSendings table
CREATE TABLE MessageSendings (
    Id INT IDENTITY(1,1) PRIMARY KEY,             -- Primary key with auto-increment
    MessageId INT NOT NULL,                       -- Foreign Key to Messages.MessageId
    SentAt DATETIME NOT NULL DEFAULT GETDATE(),   -- Date/Time the message was sent
    ConfirmationCode NVARCHAR(50) NOT NULL,       -- Confirmation code from Twilio
    FOREIGN KEY (MessageId) REFERENCES Messages(MessageId) -- Relationship to Messages table
);

-- table to store credentials
CREATE TABLE TwilioCredentials (
    Id INT IDENTITY(1,1) PRIMARY KEY,             --Primary key with auto-increment
    Accountid VARCHAR(255) NOT NULL,              --to save Twilo Account Id  
    AuthToken VARCHAR(255) NOT NULL,        	  --to save Twilo tok
    FromNumber VARCHAR(20) NOT NULL,        	  --Sending number given by Twilo
    CreatedAt DATETIME DEFAULT GETDATE()    	  --Date/Time row was created
);

INSERT INTO TwilioCredentials (Accountid, AuthToken, FromNumber)
VALUES ('AC667c548864bc96eceb20368fde8e93b3', '04f78a9badf2a3ae312f71a174f14b37', '+12694639583');
GO