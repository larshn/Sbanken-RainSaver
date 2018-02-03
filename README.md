# Nå regner det penger!
Mikrosparing er en tjeneste som mange banker har tilbyr. Sbanken har også lansert sin egen løsning for [mikrosparing](https://sbanken.no/spare/mikrosparing/)
I all hovedsak har mikrospar hittil vært knyttet opp mot kortbruk og betaling. Hver gang du bruker kortet flyttes et avtalt beløp over til din sparekonto: for eksempel: «Spar 10,- hver gang du bruker kortet», «rund av til nærmeste 50-lapp, og spar differansen» eller «spar 5,- hver gang du betaler en regning». 
Men hvorfor bare basere sparing på bruk kort eller betaling? I januar lanserte Sbanken sin [utviklerportal](https://sbanken.no/bruke/utviklerportalen/) der vi gir våre egne kunder tilgang til å «bygge sin egen nettbank». Dersom du har litt kunnskap om programmering kan du derfor enkelt lage din egen mikrosparløsning hvor selve sparingen baserer seg på helt andre faktorer enn for eksempel kortbruk.

## Azure functions
Tjenesten min er ganske enkel. Hver morgen henter programmet (som kjører som en Azure Function) dagens værmelding fra api.yr.no. Her summers hvor mange millimeter nedbør som er meldt neste 24 timer og overfører beløpet til min konto for solskinnsdager.
[Microsoft](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-azure-function) har en fin guide for å komme i gang med Azure functions.


## Værvarsel fra Yr levert av Meteorologisk institutt og NRK
Koden tar utgangspunkt i postnummer for å finne sted for værmelding. Koden kan enkelt skrives om til å ta utgangspunkt i et stedsnavn eller lengde- og breddegrad ved å bruke noen av de andre [API-ene fra Yr](http://om.yr.no/verdata/).  Dersom du ikke ønsker å ta sorgene på forskudd og basere deg på på værmeldingen kan du heller ta utgangspunkt i en litt avansert [nedbørsmåler](https://www.netatmo.com/en-US/product/weather/weatherstation/accessories#raingauge)  som kan kobles på internett og har API-støtte.


## Saving for a sunny day

A c# example using [Sbanken API](https://sbanken.no/bruke/utviklerportalen/).

This is a Weather-based micro saving service using APIs from Sbanken.no and Weather forecast from [Yr](http://yr.no), delivered by the Norwegian Meteorological Institute and NRK. For each mm rainfall, money is transferred to a savings account.

The service is set up as an Azure function with a timer trigger. 

## License

The MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
