import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { CountryCheckboxInterface, CountryInterface } from '../../interfaces/user/country.interface';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { CommonApiEnum } from '../../../core/enums/common-api.enum';
import { HttpService } from '../../../core/services/http.service';
import { GeographiesHelper } from '../../geographies-helpers/geographies.helper';

@Component({
  selector: 'neo-geographic-interest',
  templateUrl: 'geographic-interest.component.html',
  styleUrls: ['./geographic-interest.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GeographicInterestComponent implements OnChanges {
  @Input() preSelectedRegions: CountryInterface[];
  @Input() continents: CountryCheckboxInterface[];
  @Input() invitedCountries: CountryInterface[];

  @Output() selectedRegionsChange: EventEmitter<TagInterface[]> = new EventEmitter<TagInterface[]>();
  @Output() selectedContinentsChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  searchInput: FormControl = new FormControl();
  selectedContinents: CountryCheckboxInterface[] = [];
  countries: CountryCheckboxInterface[];
  initialCountriesList: CountryCheckboxInterface[];
  states: TagInterface[];
  selectedRegions: CountryInterface[] = [];
  commonApiEnum = CommonApiEnum;
  continentSelected: boolean;
  showCountriesSection: boolean;
  allUsSlug: string = 'us-all';
  fragmentOfAllUsStatesSlug: string = 'us-';
  geographiesHelper = GeographiesHelper;

  constructor(private httpService: HttpService, private changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit(): void {
    if (this.selectedContinents?.length > 0) {
      this.loadMore();
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (
      !changes['preSelectedRegions']?.previousValue &&
      changes['preSelectedRegions']?.currentValue &&
      !this.countries &&
      this.continents
    ) {
      this.preSelectedRegions.forEach(pc => {
        this.selectPreselectedContinent(pc);
      });

      this.selectedRegions = [...this.preSelectedRegions];
      this.selectedRegionsChange.emit([...this.preSelectedRegions]);

      if (this.selectedRegions.length && this.selectedRegions.some(sr => sr.parentId !== 0)) {
        this.getCountries();
      }
    } else if (changes['continents'] && changes['continents']?.currentValue?.filter(c => c.selected === true).length) {
      this.continentSelected = changes['preSelectedRegions']?.currentValue?.length > 0;

      if (this.continentSelected) {
        this.selectedContinents = changes['continents']?.currentValue?.filter(c => c.selected === true);
      }
      this.changeDetectorRef.detectChanges();
    }
  }

  selectContinent(cont: CountryInterface, index: number): void {
    this.continents[index].selected = !this.continents[index].selected;

    if (!this.continents[index].selected) {
      this.searchInput.patchValue('');
      this.countries = this.initialCountriesList != undefined ? [...this.initialCountriesList] : [];

      if (this.countries) {
        this.countries = this.countries?.filter(
          c => c.parentId !== this.continents[index].id && c.id !== this.continents[index].id
        );
        this.initialCountriesList = [...this.countries];
      }

      this.selectedRegions = this.selectedRegions?.filter(
        c => c.parentId !== this.continents[index].id && c.id !== this.continents[index].id
      );

      if (
        this.selectedRegions.some(
          el => el.parentId === this.continents[index].id || el.id === this.continents[index].id
        )
      ) {
        this.selectedRegions.forEach((item: CountryInterface, index: number, object: CountryInterface[]) => {
          if (object[index].id === this.continents[index].id && object[index].parentId === null) {
            object.splice(index, 1);
          }
        });
      }
    } else if (
      !this.selectedRegions.some(el => el.parentId === this.continents[index].id || el.id === this.continents[index].id)
    ) {
      this.selectedRegions.push({
        ...this.continents[index]
      });
    }
    this.selectedRegionsChange.emit([...this.selectedRegions]);

    this.selectedContinents = this.continents.filter(c => c.selected === true);

    if (!this.selectedContinents.length) {
      this.countries = [];
      this.initialCountriesList = [];
    } else {
      this.continentSelected = true;
    }
    if (this.continents[index].selected == true) {
      this.loadMore();
    }
  }

  loadMore(): void {
    this.searchInput.patchValue('');
    this.getCountries();
  }

  getCountries(): void {
    const selectedContinentsId: string = this.selectedContinents.map(c => c.id).join(',');
    this.httpService
      .get<TagInterface[]>(this.commonApiEnum.Regions, {
        FilterBy: `parentids=${selectedContinentsId}`
      })
      .subscribe(c => {
        c.forEach(country => {
          this.selectedRegions.forEach(c => {
            if (c.id === country.id) {
              country.selected = true;
            }
          });
        });

        this.countries = [...c];
        this.initialCountriesList = [...this.countries];
        this.showCountriesSection = true;

        if (this.selectedRegions.some(sr => sr.slug === this.allUsSlug)) {
          this.initialCountriesList.forEach(ic => {
            if (ic.slug.includes(this.fragmentOfAllUsStatesSlug) && !ic.slug.includes(this.allUsSlug)) {
              ic.disabled = true;
              ic.selected = true;
            }
          });
        }

        this.changeDetectorRef.markForCheck();
      });

    this.continentSelected = false;
  }

  chooseCountry(country: CountryCheckboxInterface): void {
    // * countries/states tags
    if (this.selectedRegions.some(c => c.id === country.id)) {
      this.selectedRegions = this.selectedRegions.filter(c => c.id !== country.id);
    } else {
      this.selectedRegions.push(country);
    }

    // * checkboxes
    this.countries.map(c => {
      if (c.id === country.id) {
        c.selected = !c.selected;
      }
      return c;
    });

    this.countries.forEach(c => {
      // * disable all states if us-all selected
      if (
        country.slug === this.allUsSlug &&
        country.selected === true &&
        c.slug.includes(this.fragmentOfAllUsStatesSlug) &&
        c.slug !== this.allUsSlug
      ) {
        c.disabled = true;
        c.selected = false;
      } else if (
        country.slug === this.allUsSlug &&
        country.selected === false &&
        c.slug.includes(this.fragmentOfAllUsStatesSlug)
      ) {
        c.disabled = false;
        c.selected = false;
      }
    });

    if (country.slug === this.allUsSlug && country.selected === true) {
      this.onUsAllSelect(country);
    }

    // * remove continent from selected regions if country from continent is selected
    if (this.selectedRegions.some(sr => sr.id === country.parentId)) {
      this.selectedRegions = this.selectedRegions.filter(sr => sr.id !== country.parentId);
    }

    // * handle unselecting continent if all countries was removed with checkboxes
    if (country.parentId !== 0 && !this.selectedRegions.some(c => c.parentId === country.parentId)) {
      this.continents.forEach(cont => {
        if (cont.id === country.parentId) cont.selected = false;
      });

      this.selectedContinents = this.selectedContinents.filter(sc => {
        return sc.id !== country.parentId;
      });

      this.countries = this.countries.filter(c => c.parentId !== country.parentId);
      this.initialCountriesList = this.initialCountriesList.filter(ic => ic.parentId !== country.parentId);
    }

    this.selectedRegionsChange.emit([...this.selectedRegions]);
  }

  onUsAllSelect(usAll: CountryCheckboxInterface): void {
    this.selectedRegions = this.selectedRegions.filter(sc => !sc.slug.includes(this.fragmentOfAllUsStatesSlug));

    this.initialCountriesList.forEach(ic => {
      if (ic.slug.includes(this.fragmentOfAllUsStatesSlug) && !ic.slug.includes(this.allUsSlug)) {
        ic.disabled = true;
        ic.selected = true;
      }
    });

    this.selectedRegions.push(usAll);
  }

  removeRegion(region: CountryInterface) {
    region.parentId === 0 ? this.removeContinent(region) : this.removeCountry(region);
    this.selectedRegionsChange.emit([...this.selectedRegions]);
  }

  removeCountry(country: CountryInterface): void {
    this.countries.forEach(c => {
      if (c.id === country.id) c.selected = false;

      if (country.slug === this.allUsSlug && c.slug.includes(this.fragmentOfAllUsStatesSlug)) {
        c.selected = false;
        c.disabled = false;
      }
    });

    this.selectedRegions = this.selectedRegions.filter(c => c.id !== country.id);

    // * remove continent if no countries from it is selected
    if (!this.selectedRegions.some(sr => sr.parentId === country.parentId)) {
      this.selectedContinents = this.selectedContinents.filter(sc => {
        return sc.id !== country.parentId;
      });

      this.countries = this.countries?.filter(c => c.parentId !== country.parentId);
      this.initialCountriesList = this.initialCountriesList?.filter(c => c.parentId !== country.parentId);

      for (let i = 0; i < this.continents.length; i++) {
        if (this.continents[i].id === country.parentId) {
          this.continents[i].selected = false;
          break;
        }
      }
    }
  }

  removeContinent(continent: CountryInterface): void {
    this.continents.forEach(c => {
      if (c.id === continent.id) c.selected = false;
    });

    // * remove continent and all its countries from selected regions
    this.selectedContinents = this.selectedContinents.filter(sc => {
      return sc.parentId !== continent.id && continent.id !== sc.id;
    });

    this.selectedRegions = this.selectedRegions.filter(c => c.id !== continent.id);
    this.countries = this.countries?.filter(c => c.parentId !== continent.id);
    this.initialCountriesList = this.initialCountriesList?.filter(c => c.parentId !== continent.id);
  }

  clearAllCountries() {
    this.continents?.forEach(c => (c.selected = false));
    this.countries = [];
    this.initialCountriesList = [];
    this.selectedContinents = [];
    this.selectedRegions = [];

    this.selectedRegionsChange.emit([...this.selectedRegions]);
  }

  anySelectedActiveCountries(): boolean {
    return this.selectedRegions.some(sc => !sc.isInvited && sc.parentId);
  }

  searchCountries(searchStr: string): void {
    this.countries = searchStr
      ? this.initialCountriesList?.filter(c => c.name.toLowerCase().includes(searchStr.toLowerCase()))
      : [...this.initialCountriesList];
  }

  private selectPreselectedContinent(region: CountryInterface): void {
    this.continents?.map(cont => {
      if (region.id === cont.id || region.parentId === cont.id) {
        cont.selected = true;
      }
    });

    if (this.continents) {
      this.selectedContinents = [
        ...this.selectedContinents,
        ...this.continents.filter(cont => cont.id === region.parentId || cont.id === region.id)
      ];
    }
  }

  countSelectedCountriesInContinent(continent: CountryCheckboxInterface): number | 'All' {
    if (
      continent.parentId === 0 &&
      this.selectedRegions.length &&
      !this.selectedRegions.some(sr => sr.parentId === continent.id)
    ) {
      return 'All';
    }
    return this.selectedRegions.filter(sr => sr.parentId === continent.id).length;
  }

  calculateSelectedContinentsStatus(): boolean {
    return this.continents.some(c => c.selected === true);
  }
}
