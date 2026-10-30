
IF COL_LENGTH('dbo.ReliefProjects', 'LastUpdated') IS NULL
BEGIN
    ALTER TABLE dbo.ReliefProjects
    ADD LastUpdated DATETIME2 NULL;

    PRINT 'LastUpdated column added successfully.';
END
ELSE
BEGIN
    PRINT 'LastUpdated column already exists.';
END;
