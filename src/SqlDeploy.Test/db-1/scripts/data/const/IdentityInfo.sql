if not exists (select 1 from dbo.IdentityInfo)
begin
	insert into dbo.IdentityInfo (Hi) values (1000)
end