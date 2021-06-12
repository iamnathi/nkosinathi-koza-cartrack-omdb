USE `omdb`;

CREATE TABLE `omdb`.`movies` (
  `IMDbID` VARCHAR(20) NOT NULL,
  `Title` VARCHAR(100) NOT NULL,
  `Type` VARCHAR(15) NOT NULL,
  `Year` VARCHAR(10) NOT NULL,
  `Rated` VARCHAR(50) NOT NULL,  
  `Released` VARCHAR(50) NOT NULL,
  `Runtime` VARCHAR(50) NOT NULL,
  `Genre` VARCHAR(100) NOT NULL,
  `Director` VARCHAR(255) NOT NULL,
  `Writer` VARCHAR(255) NOT NULL,
  `Actors` VARCHAR(255) NOT NULL,
  `Plot` VARCHAR(2000) NOT NULL,
  `Language` VARCHAR(100) NOT NULL,
  `Country` VARCHAR(100) NOT NULL,
  `Awards` VARCHAR(255) NOT NULL,
  `Poster` VARCHAR(255) NOT NULL,
  `Metascore` VARCHAR(50) NOT NULL,
  `IMDbRating` VARCHAR(50) NOT NULL,
  `IMDbVotes` VARCHAR(50) NOT NULL, 
  PRIMARY KEY (`IMDbID`)
);

CREATE TABLE `omdb`.`ratings` (
    `IMDbID` VARCHAR(20) NOT NULL,
    `Source` VARCHAR(100) NOT NULL,
	`Value` VARCHAR(15) NOT NULL,
    PRIMARY KEY (`IMDbID`, `Source`, `Value`),
    FOREIGN KEY (`IMDbID`) 
		REFERENCES `omdb`.`movies` (`IMDbID`)
        ON DELETE CASCADE
);

DELIMITER //
CREATE PROCEDURE SaveOrUpdateMovie (
	IN iMDbID VARCHAR(20),
	IN title VARCHAR(100),
	IN type VARCHAR(15),
	IN year VARCHAR(10),
	IN rated VARCHAR(50),
	IN released VARCHAR(50),
	IN runtime VARCHAR(50),
	IN genre VARCHAR(100),
	IN director VARCHAR(255),
	IN writer VARCHAR(255),
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

	INSERT INTO `omdb`.`movies` (IMDbID, Title, Type, Year, Rated, Released, Runtime, Genre, Director, Writer, Actors, Plot, Language, Country, Awards, Poster, Metascore, IMDbRating, IMDbVotes)
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

USE omdb;

DELIMITER //
CREATE PROCEDURE GetAllMovies ()
BEGIN

	SELECT
		*
	FROM omdb.movies AS m
	INNER JOIN omdb.ratings AS r
		ON m.IMDbID = r.IMDbID;

END//

DELIMITER ;


USE omdb;

DELIMITER //
CREATE PROCEDURE SaveOrUpdateMovieRating (
	IN iMDbID VARCHAR(20),
	IN source VARCHAR(100),
	IN value VARCHAR(20)
)
BEGIN

	INSERT IGNORE  INTO `omdb`.`ratings` (IMDbID, Source, Value)
    VALUES (iMDbID, source, value);

END//

DELIMITER ;


USE omdb;

DELIMITER //
CREATE PROCEDURE DeleteMovieById (
	IN imdbId VARCHAR(20)
)
BEGIN

	DELETE FROM `omdb`.`movies`
    WHERE IMDbID = imdbId;

END//

DELIMITER ;