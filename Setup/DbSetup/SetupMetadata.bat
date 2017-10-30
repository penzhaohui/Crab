sqlcmd -S localhost -E -i MetadataDatabase.sql
sqlcmd -S localhost -E -d CrabMetadata -i MetadataSchema.sql
sqlcmd -S localhost -E -d CrabMetadata -i MetadataData.sql
