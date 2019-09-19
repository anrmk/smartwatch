	/*
	1) Create Trigger Functions: updatedevicelocation
	2) on table DEVICELOCATIONS create trigger triggerdevicelastlocation
	3) Select trigger function on Definition tab as "public.updatedevicelocation"
	4) Check Fires AFTER INSERT on EVENTS tab
	*/

	BEGIN
	   IF (SELECT COUNT(*) FROM public."DeviceLastLocations" where "DeviceEntity_Id" = NEW."DeviceEntity_Id") = 0 THEN
		   INSERT INTO public."DeviceLastLocations" ("DeviceEntity_Id", "Latitude", "Longitude", "Accuracy", "Altitude", "IsFromMockProvider", "Timestamp", "Code", "Name", "Speed", "Direction")
		   VALUES (NEW."DeviceEntity_Id", NEW."Latitude", NEW."Longitude", NEW."Accuracy", NEW."Altitude", NEW."IsFromMockProvider", NEW."Timestamp", NEW."Code", NEW."Name", NEW."Speed", NEW."Direction");
	   ELSE
			UPDATE public."DeviceLastLocations"
			SET "DeviceEntity_Id" = NEW."DeviceEntity_Id",
				"Latitude" = NEW."Latitude", 
				"Longitude"= NEW."Longitude", 
				"Accuracy" = NEW."Accuracy", 
				"Altitude" = NEW."Altitude",
				"IsFromMockProvider" = NEW."IsFromMockProvider",
				"Timestamp" = NEW."Timestamp",
				"Code" = NEW."Code",
				"Name"= NEW."Name",
				"Speed"= NEW."Speed",
				"Direction"= NEW."Direction"
			WHERE "DeviceEntity_Id" = NEW."DeviceEntity_Id";
		END IF;
	   RETURN NEW;
	END;