21/11 

Startade projektet och läste igenom hela kravspecifikationen. Bestämde projektstrukturen enligt instruktionerna: ett Core-lager (logik), DataAccess-lager (Entity Framework), och UI-projektet där programmet körs. 

22/11 

Arbetade med CSV-inläsning. Implementerade CsvHelper med korrekt kulturformat, mapping av kolumner samt validering av temperatur, luftfuktighet och datum. Säkerställde att tomma eller felaktiga värden hanteras. 
Importerade hela Excel filen till databasen. 

 

23/11 

Implementerade analyserna för: 

medeltemperatur per dag 

sortering av varmaste dagar 

sortering av torraste dagar 

mögelrisk (egen formel baserat på temp/fukt) 

24/11 

La till beräkningar: 

sortering efter minst/störst mögelrisk 

dagliga genomsnittsberäkningar per plats 

temperaturdifferenser mellan inne och ute 

alla beräkningar körs automatiskt vid programmets start 

Programmet genererar en komplett sammanfattning direkt i konsolen utan meny. 

 

27/11 

Lade till logik för meteorologisk höst och vinter baserat på dygnsmedeltemperatur: 

höst: 5 dygn mellan 0–10°C 

vinter: 5 dygn ≤ 0°C 

 

28/11  

 

Gick igenom kravspecifikationen och kontrollerade att alla G-krav är uppfyllda: 
CSV-inläsning, databas, LINQ-analyser, sorteringar, meteorologisk höst/vinter. 
Förberedde projektet för inlämning. 

 

 

 
