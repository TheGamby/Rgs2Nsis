WriteRegStr HKCR "TestApp.TestApp.1" "" "TestApp Class"
WriteRegStr HKCR "TestApp.TestApp.1\CLSID" "" "{45B9C1EF-8636-441F-9E2F-59B248A704E8}"
WriteRegStr HKCR "TestApp.TestApp" "" "TestApp Class"
WriteRegStr HKCR "TestApp.TestApp\CLSID" "" "{45B9C1EF-8636-441F-9E2F-59B248A704E8}"
WriteRegStr HKCR "TestApp.TestApp\CurVer" "" "TestApp.TestApp.1"
WriteRegStr HKCR "CLSID\{45B9C1EF-8636-441F-9E2F-59B248A704E8}" "" "TestApp Class"
WriteRegStr HKCR "CLSID\{45B9C1EF-8636-441F-9E2F-59B248A704E8}\ProgID" "" "TestApp.TestApp.1"
WriteRegStr HKCR "CLSID\{45B9C1EF-8636-441F-9E2F-59B248A704E8}\VersionIndependentProgID" "" "TestApp.TestApp"
WriteRegStr HKCR "CLSID\{45B9C1EF-8636-441F-9E2F-59B248A704E8}\InprocServer32" "" "%MODULE%"
WriteRegStr HKCR "CLSID\{45B9C1EF-8636-441F-9E2F-59B248A704E8}\InprocServer32\ThreadingModel" "" "Apartment"
WriteRegStr HKCR "CLSID\{45B9C1EF-8636-441F-9E2F-59B248A704E8}\TypeLib" "" "{26410352-9147-4698-94D4-49DB9691C67C}"
