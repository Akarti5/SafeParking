# 🅿️ SafeParking - Système de Gestion de Parking

Un système de gestion de parking moderne développé en C# Windows Forms avec base de données SQL Server LocalDB.

## 📋 Table des Matières

- [Fonctionnalités](#fonctionnalités)
- [Prérequis](#prérequis)
- [Installation](#installation)
- [Configuration de la Base de Données](#configuration-de-la-base-de-données)
- [Lancement de l'Application](#lancement-de-lapplication)
- [Utilisation](#utilisation)
- [Structure du Projet](#structure-du-projet)
- [Dépannage](#dépannage)
- [Contribution](#contribution)

## ✨ Fonctionnalités

- 🔐 **Système d'authentification** (Login/Signup)
- 📊 **Dashboard en temps réel** avec statistiques
- 🚗 **Gestion des véhicules** (Ajout, suivi, sortie)
- 🅿️ **50 places de parking** (P001-P050)
- 💰 **Calcul automatique des tarifs** (20 Ariary/min pour Motor, 30 Ariary/min pour Vehicle)
- 🎫 **Génération de tickets** d'entrée et de sortie
- 🔍 **Recherche et filtrage** des véhicules
- 🗺️ **Carte visuelle** des places de parking
- ⏰ **Mise à jour en temps réel** des données

## 🛠️ Prérequis

### Logiciels Requis

1. **Visual Studio 2022** (Community, Professional ou Enterprise)
   - Télécharger : https://visualstudio.microsoft.com/fr/downloads/

2. **.NET 6.0 SDK**
   - Télécharger : https://dotnet.microsoft.com/download/dotnet/6.0

3. **SQL Server Express LocalDB**
   - Inclus avec Visual Studio ou télécharger séparément
   - Télécharger : https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads

### Vérification des Prérequis

Ouvrez l'invite de commande et vérifiez :

```bash
# Vérifier .NET
dotnet --version

# Vérifier LocalDB
sqllocaldb info
```

## 📥 Installation

### Méthode 1 : Avec Visual Studio (Recommandée)

1. **Télécharger le projet**
   - Clonez ou téléchargez le code source
   - Extrayez dans un dossier de votre choix

2. **Ouvrir Visual Studio 2022**

3. **Créer un nouveau projet**
   - Fichier → Nouveau → Projet
   - Sélectionnez "Application Windows Forms (.NET)"
   - Nom : `ParkingManagementSystem`
   - Cliquez sur "Créer"

4. **Remplacer les fichiers**
   - Supprimez `Form1.cs` et `Form1.Designer.cs`
   - Copiez tous les fichiers téléchargés dans le dossier du projet
   - Clic droit sur le projet → Ajouter → Élément existant
   - Sélectionnez tous les fichiers `.cs`

5. **Mettre à jour le fichier projet**
   
   Remplacez le contenu de `ParkingManagementSystem.csproj` par :
   
   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
     <PropertyGroup>
       <OutputType>WinExe</OutputType>
       <TargetFramework>net6.0-windows</TargetFramework>
       <UseWindowsForms>true</UseWindowsForms>
       <ImplicitUsings>enable</ImplicitUsings>
     </PropertyGroup>
     <ItemGroup>
       <PackageReference Include="System.Data.SqlClient" Version="4.8.5" />
     </ItemGroup>
   </Project>
   ```

### Méthode 2 : Avec la ligne de commande

1. **Créer le projet**
   ```bash
   dotnet new winforms -n ParkingManagementSystem
   cd ParkingManagementSystem
   ```

2. **Ajouter les packages requis**
   ```bash
   dotnet add package System.Data.SqlClient
   ```

3. **Copier les fichiers source**
   - Copiez tous les fichiers `.cs` dans le dossier du projet

## 🗄️ Configuration de la Base de Données

### Étape 1 : Démarrer LocalDB

Ouvrez l'invite de commande en tant qu'administrateur :

```bash
# Créer l'instance LocalDB (si nécessaire)
sqllocaldb create MSSQLLocalDB

# Démarrer LocalDB
sqllocaldb start MSSQLLocalDB

# Vérifier le statut
sqllocaldb info MSSQLLocalDB
```

Vous devriez voir `State: Running`.

### Étape 2 : Créer la Base de Données (Automatique)

L'application créera automatiquement la base de données au premier lancement. Si vous préférez la créer manuellement :

```sql
-- Connexion à LocalDB
sqlcmd -S "(localdb)\\MSSQLLocalDB"

-- Créer la base de données
CREATE DATABASE ParkingManagementDB;
GO

-- Utiliser la base de données
USE ParkingManagementDB;
GO
```

### Étape 3 : Structure de la Base de Données

Les tables suivantes seront créées automatiquement :

#### Table Users
```sql
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETDATE()
);
```

#### Table ParkingPlaces
```sql
CREATE TABLE ParkingPlaces (
    PlaceID INT PRIMARY KEY,
    PlaceNumber NVARCHAR(10) NOT NULL, -- P001, P002, etc.
    IsOccupied BIT DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE()
);
```

#### Table Vehicles
```sql
CREATE TABLE Vehicles (
    VehicleID INT IDENTITY(1,1) PRIMARY KEY,
    TicketID NVARCHAR(20) UNIQUE NOT NULL,
    OwnerName NVARCHAR(100) NOT NULL,
    VehicleNumber NVARCHAR(20) NOT NULL,
    VehicleType NVARCHAR(20) NOT NULL, -- 'Motor' ou 'Vehicle'
    ParkingPlaceID INT NOT NULL,
    EntryTime DATETIME NOT NULL,
    ExitTime DATETIME NULL,
    AmountPerMinute DECIMAL(10,2) NOT NULL, -- 20 pour Motor, 30 pour Vehicle
    TotalAmount DECIMAL(10,2) NULL,
    IsPaid BIT DEFAULT 0,
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (ParkingPlaceID) REFERENCES ParkingPlaces(PlaceID)
);
```

### Étape 4 : Données Initiales

L'application insère automatiquement :

- **Utilisateur admin** : username=`admin`, password=`admin123`
- **50 places de parking** : P001 à P050

## 🚀 Lancement de l'Application

### Avec Visual Studio

1. **Ouvrir le projet** dans Visual Studio
2. **Appuyer sur F5** ou cliquer sur "Démarrer"
3. **L'application se compile et se lance**

### Avec la ligne de commande

```bash
# Naviguer vers le dossier du projet
cd ParkingManagementSystem

# Compiler le projet
dotnet build

# Lancer l'application
dotnet run
```

### Premier Lancement

1. **L'écran de connexion apparaît**
2. **Connectez-vous avec :**
   - **Nom d'utilisateur :** `admin`
   - **Mot de passe :** `admin123`
3. **Le dashboard principal s'ouvre**

## 📖 Utilisation

### 1. Connexion/Inscription

- **Connexion :** Utilisez admin/admin123 ou créez un compte
- **Inscription :** Cliquez sur "Sign Up" pour créer un nouveau compte

### 2. Dashboard

- **Statistiques en temps réel :** Véhicules du jour, argent gagné, places libres/occupées
- **Carte des places :** Vue visuelle des 50 places de parking
- **Mise à jour automatique :** Toutes les 30 secondes

### 3. Ajouter un Véhicule

1. **Cliquez sur "Add Vehicle"**
2. **Remplissez les informations :**
   - Nom du propriétaire
   - Numéro du véhicule
   - Type (Motor/Vehicle)
   - Place de parking disponible
3. **Cliquez "Add Vehicle"**
4. **Un ticket est généré automatiquement**

### 4. Voir les Véhicules Garés

- **Cliquez sur "Park"**
- **Voir tous les véhicules actifs**
- **Rechercher par nom, numéro ou place**
- **Cliquer "Info" pour voir les détails et le montant actuel**

### 5. Sortie de Véhicule

1. **Cliquez sur "Exit"**
2. **Entrez l'ID du ticket** (format: TK-YYYYMMDD-XXXX)
3. **Cliquez "Search"**
4. **Vérifiez les informations et le montant**
5. **Cliquez "Leave Now"**
6. **Un ticket de sortie est généré**

## 📁 Structure du Projet

```
ParkingManagementSystem/
├── 📄 Program.cs                    # Point d'entrée de l'application
├── 🗄️ DatabaseHelper.cs            # Gestion de la base de données
├── 📁 Forms/
│   ├── 🔐 LoginForm.cs             # Formulaire de connexion
│   ├── 📝 SignUpForm.cs            # Formulaire d'inscription
│   └── 🏠 MainForm.cs              # Formulaire principal
├── 📁 UserControls/
│   ├── 📊 DashboardControl.cs      # Tableau de bord
│   ├── ➕ AddVehicleControl.cs     # Ajout de véhicule
│   ├── 🅿️ ParkControl.cs          # Vue des véhicules garés
│   └── 🚪 ExitControl.cs          # Sortie de véhicule
├── 📁 Models/
│   ├── 👤 User.cs                  # Modèle utilisateur
│   ├── 🚗 Vehicle.cs               # Modèle véhicule
│   └── 🅿️ ParkingPlace.cs         # Modèle place de parking
├── 📁 Services/
│   ├── 👥 UserService.cs           # Service utilisateur
│   ├── 🚗 VehicleService.cs        # Service véhicule
│   ├── 🅿️ ParkingPlaceService.cs  # Service places de parking
│   └── 📊 DashboardService.cs      # Service dashboard
└── 📄 README.md                    # Ce fichier
```

## 🔧 Dépannage

### Problème : "Connection to database failed"

**Solutions :**

1. **Vérifier LocalDB :**
   ```bash
   sqllocaldb info MSSQLLocalDB
   ```
   Si `State: Stopped`, lancez :
   ```bash
   sqllocaldb start MSSQLLocalDB
   ```

2. **Vérifier la chaîne de connexion** dans `DatabaseHelper.cs`

3. **Réinstaller SQL Server Express LocalDB**

### Problème : "dotnet command not found"

**Solution :**
- Installez .NET 6.0 SDK depuis https://dotnet.microsoft.com/download

### Problème : Erreurs de compilation

**Solutions :**
1. **Restaurer les packages :**
   ```bash
   dotnet restore
   ```

2. **Vérifier les références** dans le fichier `.csproj`

3. **Nettoyer et reconstruire :**
   ```bash
   dotnet clean
   dotnet build
   ```

### Problème : Police Poppins non trouvée

**Solution :**
- L'application utilisera la police système par défaut
- Pour installer Poppins : téléchargez depuis Google Fonts

## 🎯 Fonctionnalités Avancées

### Tarification

- **Motor :** 20 Ariary par minute
- **Vehicle :** 30 Ariary par minute
- **Calcul :** Arrondi à la minute supérieure

### Format des Tickets

- **ID Ticket :** TK-YYYYMMDD-XXXX (ex: TK-20241203-0001)
- **Places :** P001 à P050

### Mise à Jour Temps Réel

- **Dashboard :** Toutes les 30 secondes
- **Vue Park :** Toutes les 30 secondes
- **Date/Heure :** Toutes les secondes

## 🤝 Contribution

Pour contribuer au projet :

1. **Fork** le repository
2. **Créez une branche** pour votre fonctionnalité
3. **Committez** vos changements
4. **Poussez** vers la branche
5. **Ouvrez une Pull Request**

## 📞 Support

En cas de problème :

1. **Vérifiez** la section [Dépannage](#dépannage)
2. **Consultez** les logs dans la console
3. **Ouvrez une issue** sur le repository

## 📄 Licence

Ce projet est sous licence MIT. Voir le fichier LICENSE pour plus de détails.

---

**Développé avec ❤️ pour la gestion moderne des parkings**

🚗 **SafeParking** - Votre solution de gestion de parking intelligente
```

Ce README.md complet guide l'utilisateur étape par étape pour installer, configurer et utiliser le système de gestion de parking. Il inclut :

- ✅ **Instructions d'installation détaillées**
- ✅ **Configuration de la base de données**
- ✅ **Requêtes SQL nécessaires**
- ✅ **Guide d'utilisation complet**
- ✅ **Section de dépannage**
- ✅ **Structure du projet**
- ✅ **Prérequis et vérifications**

Le fichier est formaté en Markdown avec des emojis et une structure claire pour une meilleure lisibilité.