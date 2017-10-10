CREATE  TABLE Inventory (
        InventoryID SERIAL NOT NULL,
        IsOnKSUInventory BOOLEAN NOT NULL,
        InventoryDescription VARCHAR(50) NOT NULL,
        InventoryQuantity INTEGER NOT NULL,
        InventoryReorderQuantity INTEGER NOT NULL,
        InventoryTypeID INTEGER NOT NULL,
        InventoryLocationID INTEGER NOT NULL,
        GunSerial VARCHAR(25) NULL,
        
        CONSTRAINT uk_InventoryID_1 UNIQUE (InventoryID)
        CONSTRAINT pk_Inventory PRIMARY KEY (InventoryID)
        );

CREATE  TABLE InventoryType (
        InventoryTypeID SERIAL NOT NULL,
        InventoryTypeDescription VARCHAR(50) NOT NULL,
        InventoryTypeOwner VARCHAR(50) NOT NULL,
        
        CONSTRAINT uk_InventoryTypeID_1 UNIQUE (InventoryTypeID),
        CONSTRAINT pk_InventoryTypeID PRIMARY KEY (InventoryTypeID)
        );

CREATE  TABLE InventoryLocation (
        InventoryLocationID SERIAL NOT NULL,
        InventoryLocationDescription VARCHAR(20) NOT NULL,
        
        CONSTRAINT uk_InventoryLocationID_1 UNIQUE (InventoryLocationID),
        CONSTRAINT pk_InventoryLocationID PRIMARY KEY (InventoryLocationID)
        );

CREATE  TABLE Issued (
        IssuedID SERIAL NOT NULL,
        IssuedQuantity INTEGER NOT NULL,
        IssuedDate INTEGER NOT NULL,
        ReturnedDate INTEGER NOT NULL,
        UsedOrLostDate INTEGER NOT NULL,
        InventoryID INTEGER NOT NULL,
        IssuedToID INTEGER NOT NULL,

        CONSTRAINT uk_IssuedID_1 UNIQUE (IssuedID)
        CONSTRAINT pk_IssuedID PRIMARY KEY (IssuedID)
        );

CREATE  TABLE IssuedTo (
        IssuedToID SERIAL NOT NULL,
        IssuedToDescription VARCHAR(50) NOT NULL,
        IssuedToTypeID INTEGER NOT NULL,
        
        CONSTRAINT uk_IssuedToID_1 UNIQUE (IssuedToID),
        CONSTRAINT pk_IssuedToID PRIMARY KEY (IssuedToID)
        );

CREATE  TABLE IssuedToType (
        IssuedToTypeID SERIAL NOT NULL,
        IssuedToTypeDescription VARCHAR(50) NOT NULL,
        
        CONSTRAINT uk_IssuedToTypeID_1 UNIQUE (IssuedToTypeID),
        CONSTRAINT pk_IssuedToTypeID PRIMARY KEY (IssuedToTypeID)
        );

ALTER   TABLE Inventory
ADD     CONSTRAINT fk_InventoryTypeID
        FOREIGN KEY (InventoryTypeID)
        REFERENCES InventoryType (InventoryTypeID);

ALTER   TABLE Inventory
ADD     CONSTRAINT fk_InventoryLocationID
        FOREIGN KEY (InventoryLocationID)
        REFERENCES InventoryLocation (InventoryLocationID);

ALTER   TABLE Issued
ADD     CONSTRAINT fk_InventoryID
        FOREIGN KEY (InventoryID)
        REFERENCES Inventory (InventoryID);

ALTER   TABLE Issued
ADD     CONSTRAINT fk_IssuedToID
        FOREIGN KEY (IssuedToID)
        REFERENCES IssuedTo (IssuedToID);

ALTER   TABLE IssuedTo
ADD     CONSTRAINT fk_IssuedToTypeID
        FOREIGN KEY (IssuedToTypeID)
        REFERENCES IssuedToType (IssuedToTypeID);