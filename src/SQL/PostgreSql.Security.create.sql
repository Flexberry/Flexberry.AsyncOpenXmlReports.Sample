CREATE TABLE TypeWithFullAccessToTestUser (
 primaryKey UUID NOT NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE TypeWithoutAccessWithThisSecurityMode (
 primaryKey UUID NOT NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE TypeWithReadAccessToTestUser (
 primaryKey UUID NOT NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE TypeWithReadAccessToTestRole (
 primaryKey UUID NOT NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE TypeWithReadAccessToTestGroup (
 primaryKey UUID NOT NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE TypeWithFullAccessToTestRole (
 primaryKey UUID NOT NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE TypeWithoutAccessWithNoneSecurityMode (
 primaryKey UUID NOT NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE TypeWithFullAccessToTestGroup (
 primaryKey UUID NOT NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE STORMAG (
 primaryKey UUID NOT NULL,
 Name VARCHAR(80) NOT NULL,
 Login VARCHAR(50) NULL,
 Pwd VARCHAR(50) NULL,
 IsUser BOOLEAN NOT NULL,
 IsGroup BOOLEAN NOT NULL,
 IsRole BOOLEAN NOT NULL,
 ConnString VARCHAR(255) NULL,
 Enabled BOOLEAN NULL,
 Email VARCHAR(80) NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 Comment TEXT NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE STORMLG (
 primaryKey UUID NOT NULL,
 Group_m0 UUID NOT NULL,
 User_m0 UUID NOT NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE STORMI (
 primaryKey UUID NOT NULL,
 User_m0 UUID NOT NULL,
 Agent_m0 UUID NOT NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE Session (
 primaryKey UUID NOT NULL,
 UserKey UUID NULL,
 StartedAt TIMESTAMP(3) NULL,
 LastAccess TIMESTAMP(3) NULL,
 Closed BOOLEAN NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE STORMS (
 primaryKey UUID NOT NULL,
 Name VARCHAR(100) NOT NULL,
 Type VARCHAR(100) NULL,
 IsAttribute BOOLEAN NOT NULL,
 IsOperation BOOLEAN NOT NULL,
 IsView BOOLEAN NOT NULL,
 IsClass BOOLEAN NOT NULL,
 SharedOper BOOLEAN NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 Comment TEXT NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE STORMP (
 primaryKey UUID NOT NULL,
 Subject_m0 UUID NOT NULL,
 Agent_m0 UUID NOT NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE STORMF (
 primaryKey UUID NOT NULL,
 FilterText TEXT NULL,
 Name VARCHAR(255) NULL,
 FilterTypeNView VARCHAR(255) NULL,
 Subject_m0 UUID NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE STORMAC (
 primaryKey UUID NOT NULL,
 TypeAccess VARCHAR(7) NULL,
 Filter_m0 UUID NULL,
 Permition_m0 UUID NOT NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE STORMLO (
 primaryKey UUID NOT NULL,
 Class_m0 UUID NOT NULL,
 Operation_m0 UUID NOT NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE STORMLA (
 primaryKey UUID NOT NULL,
 View_m0 UUID NOT NULL,
 Attribute_m0 UUID NOT NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE STORMLV (
 primaryKey UUID NOT NULL,
 Class_m0 UUID NOT NULL,
 View_m0 UUID NOT NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));

CREATE TABLE STORMLR (
 primaryKey UUID NOT NULL,
 StartDate TIMESTAMP(3) NULL,
 EndDate TIMESTAMP(3) NULL,
 Agent_m0 UUID NOT NULL,
 Role_m0 UUID NOT NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


 ALTER TABLE STORMLG ADD CONSTRAINT FK85278b61d04e4bc794044e4957b8da88 FOREIGN KEY (Group_m0) REFERENCES STORMAG; 

 ALTER TABLE STORMLG ADD CONSTRAINT FKdabadf0edfbf48e7afe089f2263f8191 FOREIGN KEY (User_m0) REFERENCES STORMAG; 

 ALTER TABLE STORMI ADD CONSTRAINT FK3a14bd12d64a4731adf64b075f800ae4 FOREIGN KEY (User_m0) REFERENCES STORMAG; 

 ALTER TABLE STORMI ADD CONSTRAINT FK628649ca823b44369582ad1fc1353399 FOREIGN KEY (Agent_m0) REFERENCES STORMAG; 

 ALTER TABLE STORMP ADD CONSTRAINT FK8771ebbd173d4cebaac2a9f6b491ea5a FOREIGN KEY (Subject_m0) REFERENCES STORMS; 

 ALTER TABLE STORMP ADD CONSTRAINT FK75553919511a4245b664bffb589da9bb FOREIGN KEY (Agent_m0) REFERENCES STORMAG; 

 ALTER TABLE STORMF ADD CONSTRAINT FK49cfc5f2337f49e4851cda768d03e7dc FOREIGN KEY (Subject_m0) REFERENCES STORMS; 

 ALTER TABLE STORMAC ADD CONSTRAINT FKc3327c4c83af48a39bb4106b70e4f674 FOREIGN KEY (Filter_m0) REFERENCES STORMF; 

 ALTER TABLE STORMAC ADD CONSTRAINT FK4dbdccd71ebe415f8eefc0bac90f5de4 FOREIGN KEY (Permition_m0) REFERENCES STORMP; 

 ALTER TABLE STORMLO ADD CONSTRAINT FK43988b28aaa8499cbe21fb1e40c88cee FOREIGN KEY (Class_m0) REFERENCES STORMS; 

 ALTER TABLE STORMLO ADD CONSTRAINT FK1d55b1ae77fe4a4dbf0f011d686832d2 FOREIGN KEY (Operation_m0) REFERENCES STORMS; 

 ALTER TABLE STORMLA ADD CONSTRAINT FK5f33d95d5d3c44159d832dff5e47e30c FOREIGN KEY (View_m0) REFERENCES STORMS; 

 ALTER TABLE STORMLA ADD CONSTRAINT FKbff71ab155a749c4b983d01d7fb14016 FOREIGN KEY (Attribute_m0) REFERENCES STORMS; 

 ALTER TABLE STORMLV ADD CONSTRAINT FK0e9bcea0cceb464581f2cff9d0a71a91 FOREIGN KEY (Class_m0) REFERENCES STORMS; 

 ALTER TABLE STORMLV ADD CONSTRAINT FK9b5a28e310844a749c4caa10b723f99f FOREIGN KEY (View_m0) REFERENCES STORMS; 

 ALTER TABLE STORMLR ADD CONSTRAINT FK2db4f4335dc9410e943e75ced52f239a FOREIGN KEY (Agent_m0) REFERENCES STORMAG; 

 ALTER TABLE STORMLR ADD CONSTRAINT FK3f87d4b5d3ad455cafb0c7480112d8fb FOREIGN KEY (Role_m0) REFERENCES STORMAG; 
