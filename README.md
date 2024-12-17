
# Pokemon

## Prerequisites
You need to use :  
* C#  
* Windows Presentation Foundation (WPF)
* SQL server express  
  


## Installation
First you need to clone the repository  
```bash
git clone https://github.com/airoonnee/Pokemon.git
```  
and after 
```bash
git init
```
And you need to install the Package :



* BCrypt.Net-Next
* CommunityToolkit.Mvvm
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Design
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools


And finally, you need to install the 
* SQL server express
* SQL Management Studio
To manage and add Pokemon and their Spells
You must have a database identical to this one.

-- Création de la base de données

CREATE DATABASE ExerciceMonster;

GO

-- Utilisation de la base de données

USE ExerciceMonster;

GO

-- Table Login

CREATE TABLE Login (

ID INT PRIMARY KEY IDENTITY(1,1),

Username NVARCHAR(50) NOT NULL,

PasswordHash NVARCHAR(255) NOT NULL

);

-- Table Player

CREATE TABLE Player (

ID INT PRIMARY KEY IDENTITY(1,1),

Name NVARCHAR(50) NOT NULL,

LoginID INT,

FOREIGN KEY (LoginID) REFERENCES Login(ID)

);

-- Table Monster

CREATE TABLE Monster (

ID INT PRIMARY KEY IDENTITY(1,1),

Name NVARCHAR(50) NOT NULL,

Health INT NOT NULL,

ImageURL NVARCHAR(255) NULL

);

-- Table Spell

CREATE TABLE Spell (

ID INT PRIMARY KEY IDENTITY(1,1),

Name NVARCHAR(50) NOT NULL,

Damage INT NOT NULL,

Description NVARCHAR(MAX)

);

-- Table PlayerMonster (relation Player <-> Monster)

CREATE TABLE PlayerMonster (

PlayerID INT NOT NULL,

MonsterID INT NOT NULL,

PRIMARY KEY (PlayerID, MonsterID),

FOREIGN KEY (PlayerID) REFERENCES Player(ID),

FOREIGN KEY (MonsterID) REFERENCES Monster
(ID)

);

-- Table MonsterSpell (relation Monster <-> 
Spell)

CREATE TABLE MonsterSpell (

MonsterID INT NOT NULL,

SpellID INT NOT NULL,

PRIMARY KEY (MonsterID, SpellID),

FOREIGN KEY (MonsterID) REFERENCES Monster(ID),

FOREIGN KEY (SpellID) REFERENCES Spell(ID)

);


Then connect your database to the project you have opened in Microsoft Visual Studio.
```bash
dotnet tool install --global dotnet-ef
dotnet ef dbcontext scaffold
"Server=YourServer;Database=ExerciceMonster;Trusted_Connection=True;TrustServerCertificate=True;"
Microsoft.EntityFrameworkCore.SqlServer -o Model --Force
```

## Start


#### To launch the game, in Microsoft Visual Studio :  
1) Click on the green arrow  :

2) Then enter the path to your database, which should look like this:
```bash
Server=YourServer\SQLEXPRESS;Database=ExerciceMonster;Trusted_Connection=True;TrustServerCertificate=True;
```
then create an account or log in directly.

## How To play

In this game, you can see what's in your Bdd with all the spells for all the Pokémon, as well as all the Pokémon, which you can see in more detail by clicking on them. By looking at a Pokémon in detail, you can launch a random battle against another Pokémon. Your goal is to win as many rounds as possible. After each opponent's Pokémon kills, you'll have another random one come in, but this time with more life and it will inflict more damage. Opposing Pokémon attack randomly. If you have a spell that has 0 damage, it will instantly give you +20 extra HP if it's you using it, and 20 extra if it's your opponent using it.





## Author
By Erwan AGESNE
