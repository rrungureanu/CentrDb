-- Create schema
CREATE SCHEMA commerce;
go

-- Create tables
CREATE TABLE commerce.salespersons (
	salespersonId INT IDENTITY (1, 1) PRIMARY KEY,
	firstName VARCHAR (255) NOT NULL,
	familyName VARCHAR (255) NOT NULL,
	phone VARCHAR (25),
	email VARCHAR (255),
);

CREATE TABLE commerce.districts (
	districtId INT IDENTITY (1, 1) PRIMARY KEY,
	primarySalespersonId INT NOT NULL,
	name VARCHAR (255) NOT NULL,
	FOREIGN KEY (primarySalespersonId) REFERENCES commerce.salespersons (salespersonId) ON DELETE NO ACTION ON UPDATE NO ACTION
);

create table commerce.secondarySalespersonsRelations 
    (
	PRIMARY KEY (salespersonId, districtId),
    salespersonId INT NOT NULL,
	districtId INT NOT NULL,
	FOREIGN KEY (salespersonId) REFERENCES commerce.salespersons(salespersonId) ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY (districtId) REFERENCES commerce.districts(districtId) ON DELETE CASCADE ON UPDATE CASCADE
    );

CREATE TABLE commerce.stores (
	storeId INT IDENTITY (1, 1) PRIMARY KEY,
	districtId INT NOT NULL,
	name VARCHAR (255) NOT NULL,
	phone VARCHAR (25),
	city VARCHAR (255),
	FOREIGN KEY (districtId) REFERENCES commerce.districts (districtId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Adding constraint: uniqueness of values

ALTER TABLE commerce.districts
ADD CONSTRAINT UC_DistrictsName UNIQUE (name);

ALTER TABLE commerce.stores
ADD CONSTRAINT UC_StoresName UNIQUE (name);

ALTER TABLE commerce.salespersons
ADD CONSTRAINT UC_SalesPersonsPhone UNIQUE (phone);

ALTER TABLE commerce.salespersons
ADD CONSTRAINT UC_SalesPersonsEmail UNIQUE (email);

-- Adding constraint: salespersons cannot be primary and secondary for the same district

CREATE FUNCTION commerce.fn_salesperson_is_primary_and_secondary
(
    @salespersonId INT,
    @districtId INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @isPrimary BIT
    DECLARE @isSecondary BIT

    SELECT @isPrimary = CASE WHEN primarySalespersonId = @salespersonId THEN 1 ELSE 0 END
    FROM commerce.districts
    WHERE districtId = @districtId

    SELECT @isSecondary = CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
    FROM commerce.secondarySalespersonsRelations
    WHERE districtId = @districtId AND salespersonId = @salespersonId

    RETURN CASE WHEN @isPrimary = 1 AND @isSecondary = 1 THEN 1 ELSE 0 END
END

ALTER TABLE commerce.secondarySalespersonsRelations
ADD CONSTRAINT chk_salesperson_not_both_primary_and_secondary
CHECK (
    commerce.fn_salesperson_is_primary_and_secondary(salespersonId, districtId) = 0
);

-- Add procedure for getting salespersons and stores based on district

CREATE PROCEDURE commerce.getSalespersonsAndStoresByDistrictId
    @districtId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Get salespersons associated with the district as secondary salespersons
    SELECT 
        s.salespersonId, 
        s.firstName, 
        s.familyName,
		s.phone,
		s.email,
        'false' AS isPrimary
    FROM 
        commerce.salespersons s
        INNER JOIN commerce.secondarySalespersonsRelations ssr ON s.salespersonId = ssr.salespersonId
        INNER JOIN commerce.districts d ON ssr.districtId = d.districtId
    WHERE 
        d.districtId = @districtId

    UNION

    -- Get primary salesperson for the district
    SELECT 
        s.salespersonId, 
        s.firstName, 
        s.familyName,
		s.phone,
		s.email,
        'true' AS isPrimary
    FROM 
        commerce.salespersons s
        INNER JOIN commerce.districts d ON s.salespersonId = d.primarySalespersonId
    WHERE 
        d.districtId = @districtId

	-- Get stores associated with the district
	SELECT 
        s.storeId,
        s.name,
        s.phone,
        s.city
    FROM 
        commerce.stores s
        INNER JOIN commerce.districts d ON s.districtId = d.districtId
    WHERE 
        d.districtId = @districtId;

END

-- Add procedure for removing salespersons from a district

CREATE PROCEDURE commerce.RemoveSalespersonFromDistrict
    @salespersonId INT,
    @districtId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @isPrimary BIT;

    -- Check if the salesperson is primary for the district
    SELECT @isPrimary = CASE WHEN primarySalespersonId = @salespersonId THEN 1 ELSE 0 END
    FROM commerce.districts
    WHERE districtId = @districtId

    IF @isPrimary = 1
    BEGIN
        RAISERROR('Cannot remove a primary salesperson from the district', 1, 1)
        RETURN
    END

    -- Remove the secondary salesperson from the district
    DELETE FROM commerce.secondarySalespersonsRelations
    WHERE salespersonId = @salespersonId AND districtId = @districtId
    
END
GO

-- Add procedure for getting all salespersons not associated with a district

CREATE PROCEDURE commerce.getSalespersonsNotInDistrict
    @districtId INT
AS
BEGIN
    SELECT *
    FROM commerce.salespersons s
    WHERE s.salespersonId NOT IN (
        SELECT d.primarySalespersonId
        FROM commerce.districts d
        INNER JOIN commerce.salespersons ps ON d.primarySalespersonId = ps.salespersonId
        WHERE d.districtId = @districtId
        UNION
        SELECT ssr.salespersonId
        FROM commerce.secondarySalespersonsRelations ssr
        WHERE ssr.districtId = @districtId
    )
END

-- Add procedure for adding existing salesperson to a district

CREATE PROCEDURE commerce.appendSalespersonToDistrict
    @salespersonId INT,
    @districtId INT,
    @isPrimary BIT
AS
BEGIN
    IF (@isPrimary = 1)
    BEGIN
		-- Disable the constraints
		ALTER TABLE commerce.secondarySalespersonsRelations NOCHECK CONSTRAINT ALL

        -- Set the current primary salesperson for the district as a secondary salesperson
        DECLARE @currentPrimarySalespersonId INT;
        SELECT @currentPrimarySalespersonId = primarySalespersonId FROM commerce.districts WHERE districtId = @districtId;
        
		INSERT INTO commerce.secondarySalespersonsRelations
        VALUES (@currentPrimarySalespersonId, @districtId);
        
        -- Add the new salesperson as the primary salesperson for the district
        UPDATE commerce.districts
        SET primarySalespersonId = @salespersonId
        WHERE districtId = @districtId;

		-- Re-enable the constraints
		ALTER TABLE commerce.secondarySalespersonsRelations WITH CHECK CHECK CONSTRAINT ALL
    END
    ELSE
    BEGIN
        -- Add the new salesperson as a secondary salesperson for the district
        INSERT INTO commerce.secondarySalespersonsRelations
        VALUES (@salespersonId, @districtId);
    END
END

-- Add procedure for adding new salesperson to a district

CREATE PROCEDURE commerce.addNewSalespersonToDistrict
	@firstName VARCHAR (255),
	@familyName VARCHAR (255),
	@phone VARCHAR (25),
	@email VARCHAR (255), 
    @districtId INT,
    @isPrimary BIT
AS
BEGIN
    DECLARE @salespersonId INT
	-- Disable the constraints
		ALTER TABLE commerce.secondarySalespersonsRelations NOCHECK CONSTRAINT ALL

    -- Insert the new salesperson into the salespersons table
    INSERT INTO commerce.salespersons (firstName, familyName, email, phone)
    VALUES (@firstName, @familyName, @email, @phone);

    -- Get the ID of the newly inserted salesperson
    SET @salespersonId = SCOPE_IDENTITY();

    IF (@isPrimary = 1)
    BEGIN
		

        -- Set the current primary salesperson for the district as a secondary salesperson
        INSERT INTO commerce.secondarySalespersonsRelations (salespersonId, districtId)
        SELECT primarySalespersonId, @districtId FROM commerce.districts WHERE districtId = @districtId;

        -- Add the new salesperson as the primary salesperson for the district
        UPDATE commerce.districts
        SET primarySalespersonId = @salespersonId
        WHERE districtId = @districtId;

    END
    ELSE
    BEGIN
        -- Add the new salesperson as a secondary salesperson for the district
        INSERT INTO commerce.secondarySalespersonsRelations
        VALUES (@salespersonId, @districtId);
    END

	-- Re-enable the constraints
	ALTER TABLE commerce.secondarySalespersonsRelations WITH CHECK CHECK CONSTRAINT ALL
END