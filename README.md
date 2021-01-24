# PizzeriaAPI-Order-Storage VG-uppgift

## Andreea Nenciu

#### Arkitektur: 
* Pizzeria-API: ett projekt som innehåller API för order-systemet utvecklat i labb 3. Jag har ändrat det lite för att kunna koppla projektet med lagersystemet (I OrdersController finns ett httpclient som postar data till lagersystemet)
* Pizzeria-Storage-API: ett projekt som innehåller API för lagerhantering som har persistens i en SQL-databas.
* Pizzeria-Storage-Frontend: ett projekt som innehåller frontend-delen för lagersystemet i form av statiska html- och javascript-filer.
* PizzeriaTests: ett projekt som innehåller unit-tester för de 3 projekten nämnda ovanför.

#### Användning:
* Projektet körs genom docker-compose 
* För att kunna köra end-to-end-testerna: navigera till mappen e2etests i terminalen, skriv cypress open kommando i terminalen och starta pizzeriatestst.spec.js filen från Integration-mappen.

#### Designmönster motivation:
* Jag har valt att implementera visitor-designmönstret för att göra en massleverans i  lagersystemet (MassDeliveryVisitor.cs i Services mappen o Pizzeria-Storage-Api) Jag har valt just visitor design pattern för att separera logiken i IngredientItemController. Detta gör det möjligt att skapa en datamodell med begränsad intern funktionalitet och sätter upp en visitor som utför massleveransoperationen på varje data-objekt.

* Ett annat designmönster som hade varit möjligt att använda i mitt program hade varit repository design pattern som ett mellanlager mellan lager-databasen och lager-APIet. Jag valde att inte implementera repository pattern eftersom jag har använd EntityFramework som ORM för att skapa databasen. EF minskar och gömmer komplexiteten av att bygga queries i databasen. Därför kändes det onödigt och over engineering för just det här projektet som har en begränsad storlek.

### Lagersystemet - Uppgift:
#### Introduktion och syfte
Kontrollera att ni förstår, och kan självständigt, använda innehållet från materialet i kursen på en avancerad nivå. VG-uppgiften är frivillig och utförs individuellt. 
Uppgift 
Målet är att bygga ett väldigt simpelt lagersystem som integreras med er pizzasystemlösning labb3. 
Lagersystemet ska ha följande funktioner: 
* En frontend där det går att visa nuvarande antal på alla ingredienser (de ingredienser som ska finnas är de som används i labb3). Det ska gå att ändra på lagersaldot manuellt, samt ha en funktion för massleverans (öka alla ingrediensers saldo med 10). (Utseende och frontendkod bedöms inte i första hand, men det ska gå att använda systemet som användare). 
* Ett REST-interface som kallas på från er labb 3 (minimala modifikationer är tillåtna i er labb3 för att faktiskt göra requests, inga övriga ändringar ska behövas). RESTinterfacet ska “förbruka” ingredienser från lagersystemet. 
* Saldot ska lagras i någon form av persistens, fil eller databas efter egen smak. Koden ska innehålla minst ett motiverat designmönster. 

Koden ska vara enhetstestad på en rimlig nivå (bara C#-koden, inte frontendkoden eller eventuell persistenskod). 
Programmet ska köras i docker compose. Det ska spinna upp samtliga containrar som behövs (exklusive er labb3-kod). 
End-to-end-test ska finnas för att lägga till ingredienser för en pizza, sedan beställa den pizzan (göra ett anrop till ert REST-interface som används av er labb3kod) och sedan kontrollera att saldot stämmer. 

#### Redovisning 
Källkoden ska vara pushad till ett eget publikt repository på GitHub. Er labb3 som eget projekt i samma solution ska ligga med i repot, tillsammans med de andra projekt ni anser behövs för att lösa uppgiften.
Uppgiften ska genomföras individuellt och fullständigt namn ska finnas med i README.md i repots main-branch (master). 
README.md ska också innehålla en beskrivning av ett designmönster som hade varit möjligt att använda i ert program, men som inte valdes. Motivera varför det inte användes i ert program. 

#### Rättning 
Till skillnad från labbarna så godkänns uppgiften direkt eller så blir den inte godkänd. Det går inte att komplettera senare. Godkänd VG-uppgift i kombination med alla labbar och godkänd tenta ger VG i kursen. Det som bedöms är hur ren er kod är, designmönstrens motiveringar, välskrivna tester, både enhetstester och end-to-end, samt arkitekturella val i hur ni delat upp tjänster.
