import { CountryCheckboxInterface, CountryInterface } from '../interfaces/user/country.interface';

export abstract class GeographiesHelper {
  static countSelectedCountriesInContinent(
    continent: CountryCheckboxInterface,
    selectedRegions: CountryInterface[]
  ): number | 'All' {
    if (
      continent.parentId === 0 &&
      selectedRegions.length &&
      !selectedRegions.some(sr => sr.parentId === continent.id)
    ) {
      return 'All';
    }
    return selectedRegions.filter(sr => sr.parentId === continent.id).length;
  }
}
