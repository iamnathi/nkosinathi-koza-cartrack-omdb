# Getting started
Please run the below SQL script on your local instance of MySQL

```SQL
CREATE SCHEMA `omdb`;

USE `omdb`;

CREATE TABLE `omdb`.`titles` (
  `IMDbID` VARCHAR(20) NOT NULL,
  `Title` VARCHAR(100) NOT NULL,
  `Year` VARCHAR(20) NOT NULL,
  `Type` VARCHAR(15) NOT NULL,  
  `Poster` VARCHAR(255) NULL,
  `Rated` VARCHAR(50) NULL,  
  `Released` VARCHAR(50) NULL,
  `Runtime` VARCHAR(50) NULL,
  `Genre` VARCHAR(100) NULL,
  `Director` VARCHAR(255) NULL,
  `Writer` VARCHAR(4000) NULL,
  `Actors` VARCHAR(255) NULL,
  `Plot` VARCHAR(2000) NULL,
  `Language` VARCHAR(100) NULL,
  `Country` VARCHAR(100) NULL,
  `Awards` VARCHAR(255) NULL,  
  `Metascore` VARCHAR(50) NULL,
  `IMDbRating` VARCHAR(50) NULL,
  `IMDbVotes` VARCHAR(50) NULL, 
  PRIMARY KEY (`IMDbID`)
);

CREATE TABLE `omdb`.`ratings` (
    `IMDbID` VARCHAR(20) NOT NULL,
    `Source` VARCHAR(100) NOT NULL,
	`Value` VARCHAR(15) NOT NULL,
    PRIMARY KEY (`IMDbID`, `Source`, `Value`),
    FOREIGN KEY (`IMDbID`) 
		REFERENCES `omdb`.`titles` (`IMDbID`)
        ON DELETE CASCADE
);

DELIMITER //
CREATE PROCEDURE `omdb`.`SaveOrUpdateTitle` (
	IN iMDbID VARCHAR(20),
	IN title VARCHAR(100),
	IN type VARCHAR(15),
	IN year VARCHAR(10),
	IN rated VARCHAR(50),
	IN released VARCHAR(50),
	IN runtime VARCHAR(50),
	IN genre VARCHAR(100),
	IN director VARCHAR(255),
	IN writer VARCHAR(4000),
	IN actors VARCHAR(255),
	IN plot VARCHAR(2000),
	IN language VARCHAR(100),
	IN country VARCHAR(100),
	IN awards VARCHAR(255),
	IN poster VARCHAR(255),
	IN metascore VARCHAR(50),
	IN iMDbRating VARCHAR(50),
	IN iMDbVotes VARCHAR(50)
)
BEGIN

	INSERT INTO `omdb`.`titles` (IMDbID, Title, Type, Year, Rated, Released, Runtime, Genre, Director, Writer, Actors, Plot, Language, Country, Awards, Poster, Metascore, IMDbRating, IMDbVotes)
    VALUES (iMDbID, title, type, year, rated, released, runtime, genre, director, writer, actors, plot, language, country, awards, poster, metascore, iMDbRating, iMDbVotes)
    ON DUPLICATE KEY 
		UPDATE
			Title = VALUES(Title), 
            Type = VALUES(Type), 
            Year = VALUES(Year), 
            Rated = VALUES(Rated), 
            Released = VALUES(Released), 
            Runtime = VALUES(Runtime), 
            Genre = VALUES(Genre), 
            Director = VALUES(Director), 
            Writer = VALUES(Writer), 
            Actors = VALUES(Actors), 
            Plot = VALUES(Plot), 
            Language = VALUES(Language), 
            Country = VALUES(Country), 
            Awards = VALUES(Awards), 
            Poster = VALUES(Poster), 
            Metascore = VALUES(Metascore), 
            IMDbRating = VALUES(IMDbRating), 
            IMDbVotes = VALUES(IMDbVotes);

END//

DELIMITER ;

DELIMITER //
CREATE PROCEDURE `omdb`.`GetAllTitles` ()
BEGIN

	SELECT
		*
	FROM `omdb`.`titles` AS m
	LEFT OUTER JOIN `omdb`.`ratings` AS r
		ON m.IMDbID = r.IMDbID;

END//

DELIMITER ;

DELIMITER //
CREATE PROCEDURE `omdb`.`SaveOrUpdateTitleRating` (
	IN iMDbID VARCHAR(20),
	IN source VARCHAR(100),
	IN value VARCHAR(20)
)
BEGIN

	INSERT IGNORE  INTO `omdb`.`ratings` (IMDbID, Source, Value)
    VALUES (iMDbID, source, value);

END//

DELIMITER ;
```

The client console application makes use the CommandLineParser package to process commands.

The available commands are
1. Get
2. Search
3. Query
4. Create
5. Update
6. Delete

To run the program you can navigate to the project directory to run any of the commands. Example
```
dotnet run get -i "tt9140554"
```

To see the available options for a command/verb you can run the help command/verb. Example
```
dotnet run help create
```

To run OData queries, run the query command. Example 
```
dotnet run query -s "title, year"
```