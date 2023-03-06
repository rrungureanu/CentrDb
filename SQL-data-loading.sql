use CommerceDatabase;

SET IDENTITY_INSERT commerce.salespersons ON;  

INSERT INTO commerce.salespersons(salespersonId,familyName,firstName,phone,email) VALUES(1,'Jensen','Alex','+4542977893','exmail1@gmail.com')
INSERT INTO commerce.salespersons(salespersonId,familyName,firstName,phone,email) VALUES(2,'Cateman','Bob','+4542973893','exmail2@gmail.com')
INSERT INTO commerce.salespersons(salespersonId,familyName,firstName,phone,email) VALUES(3,'Kyrie','Mariana','+4542927893','exmail3@gmail.com')
INSERT INTO commerce.salespersons(salespersonId,familyName,firstName,phone,email) VALUES(4,'Axelsen','Maria','+4542917893','exmail4@gmail.com')
INSERT INTO commerce.salespersons(salespersonId,familyName,firstName,phone,email) VALUES(5,'Skov','Benjamin','+4541977893','exmail5@gmail.com')
INSERT INTO commerce.salespersons(salespersonId,familyName,firstName,phone,email) VALUES(6,'Helene','Bjorholm','+4342977893','exmail6@gmail.com')
INSERT INTO commerce.salespersons(salespersonId,familyName,firstName,phone,email) VALUES(7,'Jensen','Kasper','+45424778233','exmail7@gmail.com')

SET IDENTITY_INSERT commerce.salespersons OFF;  

SET IDENTITY_INSERT commerce.districts ON;  

INSERT INTO commerce.districts(districtId,name,primarySalespersonId) VALUES(1,'North Denmark',1)
INSERT INTO commerce.districts(districtId,name,primarySalespersonId) VALUES(2,'South Denmark',1)
INSERT INTO commerce.districts(districtId,name,primarySalespersonId) VALUES(3,'Fyn',3)
INSERT INTO commerce.districts(districtId,name,primarySalespersonId) VALUES(4,'Sealand',5)

SET IDENTITY_INSERT commerce.districts OFF;

SET IDENTITY_INSERT commerce.stores ON;  

INSERT INTO commerce.stores(storedId,districtId,name,city,phone) VALUES(1,1,'Aalborg Store','Aalborg','+4512479873')
INSERT INTO commerce.stores(storedId,districtId,name,city,phone) VALUES(2,1,'Aarhus Store','Aarhus','+4512439873')
INSERT INTO commerce.stores(storedId,districtId,name,city,phone) VALUES(3,1,'Randers Store','Randers','+4512479872')
INSERT INTO commerce.stores(storedId,districtId,name,city,phone) VALUES(4,2,'Esbjerg Store','Esbjerg','+4512419873')
INSERT INTO commerce.stores(storedId,districtId,name,city,phone) VALUES(5,2,'Kolding Store','Kolding','+4532479873')
INSERT INTO commerce.stores(storedId,districtId,name,city,phone) VALUES(6,3,'Odense Store','Odense','+4532479871')
INSERT INTO commerce.stores(storedId,districtId,name,city,phone) VALUES(7,4,'Nytorv Store','Copenhagen','+4532422873')
INSERT INTO commerce.stores(storedId,districtId,name,city,phone) VALUES(8,4,'Amager Store','Copenhagen','+4531422873')
INSERT INTO commerce.stores(storedId,districtId,name,city,phone) VALUES(8,4,'Hillerød Store','Hillerød','+4531422872')

SET IDENTITY_INSERT commerce.stores OFF; 

INSERT INTO commerce.secondarySalespersonsRelations(salespersonId,districtId) VALUES(2,1)
INSERT INTO commerce.secondarySalespersonsRelations(salespersonId,districtId) VALUES(3,1)
INSERT INTO commerce.secondarySalespersonsRelations(salespersonId,districtId) VALUES(4,1)
INSERT INTO commerce.secondarySalespersonsRelations(salespersonId,districtId) VALUES(7,2)
INSERT INTO commerce.secondarySalespersonsRelations(salespersonId,districtId) VALUES(6,2)
INSERT INTO commerce.secondarySalespersonsRelations(salespersonId,districtId) VALUES(5,3)
INSERT INTO commerce.secondarySalespersonsRelations(salespersonId,districtId) VALUES(6,3)
INSERT INTO commerce.secondarySalespersonsRelations(salespersonId,districtId) VALUES(1,5)
INSERT INTO commerce.secondarySalespersonsRelations(salespersonId,districtId) VALUES(2,5)
INSERT INTO commerce.secondarySalespersonsRelations(salespersonId,districtId) VALUES(7,5)