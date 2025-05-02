import { FormControl } from '@angular/forms';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output
} from '@angular/core';
import { filter } from 'rxjs/operators';
import { take } from 'rxjs';

import { CountryCheckboxInterface, CountryInterface } from '../../../shared/interfaces/user/country.interface';
import { GeographiesHelper } from '../../../shared/geographies-helpers/geographies.helper';
import { RegionsService } from 'src/app/shared/services/regions.service';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { CommonApiEnum } from '../../../core/enums/common-api.enum';
import { HttpService } from '../../../core/services/http.service';

@Component({
  selector: 'neo-regions-modal',
  templateUrl: 'regions-modal.component.html',
  styleUrls: ['../../../../styles/regions-selection.scss', './regions-modal.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegionsModalComponent implements OnInit, OnDestroy {
  @Input() preSelectedGeographies: CountryInterface[];

  @Output() updatedRegions: EventEmitter<CountryInterface[]> = new EventEmitter<CountryInterface[]>();
  @Output() closeModal: EventEmitter<boolean> = new EventEmitter<boolean>();

  geographiesHelper = GeographiesHelper;
  commonApiEnum = CommonApiEnum;
  searchInput: FormControl = new FormControl();
  allUsSlug: string = 'us-all';
  fragmentOfAllUsStatesSlug: string = 'us-';
  continents: CountryCheckboxInterface[];
  selectedContinents: CountryCheckboxInterface[] = [];
  selectedGeographies: CountryInterface[] = [];
  countries: CountryCheckboxInterface[];
  initialCountriesList: CountryCheckboxInterface[];

  private selectedContinentsId: string;

  constructor(
    private httpService: HttpService,
    private regionsService: RegionsService,
    private changeDetRef: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.regionsService.continents$
      .pipe(
        filter(v => !!v),
        take(1)
      )
      .subscribe(c => {
        this.continents = [...c];

        this.preSelectedGeographies?.forEach(pc => {
          // * preselect continent
          if (pc.parentId === 0) {
            this.continents.forEach(c => {
              if (c.id === pc.id) {
                c.selected = true;
              }
            });
          }

          if (pc.parentId !== 0) {
            this.continents.forEach(c => {
              if (c.id === pc.parentId) {
                c.selected = true;
              }
            });
          }
        });

        this.selectedContinents = this.continents.filter(c => c.selected === true);

        if (this.preSelectedGeographies) {
          this.selectedGeographies = [...this.preSelectedGeographies];
        }

        // * preselect country or state
        if (this.selectedGeographies.some(sr => sr.parentId !== 0)) {
          this.selectedContinentsId = this.selectedContinents.map(c => c.id).join(',');
          this.getCountries();
        }
      });
      this.loadMore();
  }

  ngOnDestroy(): void {
    this.continents.forEach(c => {
      c.selected = false;
    });
  }

  selectContinent(continent: CountryCheckboxInterface, index: number): void {
    this.continents[index].selected = !this.continents[index].selected;

    if (!this.continents[index].selected) {
      this.searchInput.patchValue('');
      this.countries = this.initialCountriesList != undefined ? this.initialCountriesList : [];
      if (this.countries) {
        this.countries = this.countries?.filter(
          // * remove countries of unselected continent from the list
          c => c.parentId !== this.continents[index].id && c.id !== this.continents[index].id
        );
        this.initialCountriesList = [...this.countries];
      }

      this.selectedGeographies = this.selectedGeographies?.filter(
        // * remove countries of unselected continent from tags area
        c => c.parentId !== this.continents[index].id && c.id !== this.continents[index].id
      );
    } else if (
      !this.selectedGeographies.some(
        el => el.parentId === this.continents[index].id || el.id === this.continents[index].id
      )
    ) {
      this.selectedGeographies.push({
        ...this.continents[index]
      });
    }

    this.selectedContinents = this.continents.filter(c => c.selected === true);
    if (this.continents[index].selected == true) {
      this.loadMore();
    }
  }

  loadMore(): void {
    this.searchInput.patchValue('');
    this.getCountries();
  }

  getCountries(): void {
    this.selectedContinentsId = this.selectedContinents.map(c => c.id).join(',');

    this.httpService
      .get<TagInterface[]>(this.commonApiEnum.Regions, {
        FilterBy: `parentids=${this.selectedContinentsId}`
      })
      .subscribe(c => {
        c.forEach(country => {
          this.selectedGeographies.forEach(c => {
            if (c.id === country.id) {
              country.selected = true;
            }
          });
        });

        this.countries = [...c];
        this.initialCountriesList = [...this.countries];

        if (this.selectedGeographies.some(sr => sr.slug === this.allUsSlug)) {
          this.initialCountriesList.forEach(ic => {
            if (ic.slug.includes(this.fragmentOfAllUsStatesSlug) && !ic.slug.includes(this.allUsSlug)) {
              ic.disabled = true;
              ic.selected = true;
            }
          });
        }

        this.changeDetRef.detectChanges();
      });
  }

  chooseCountry(country: CountryCheckboxInterface): void {
    // * countries/states tags
    if (this.selectedGeographies.some(c => c.id === country.id)) {
      this.selectedGeographies = this.selectedGeographies.filter(c => c.id !== country.id);
    } else {
      this.selectedGeographies.push(country);
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
        c.selected = true;
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
    if (this.selectedGeographies.some(sr => sr.id === country.parentId)) {
      this.selectedGeographies = this.selectedGeographies.filter(sr => sr.id !== country.parentId);
    }

    // * handle unselecting continent if all countries was removed with checkboxes
    if (country.parentId !== 0 && !this.selectedGeographies.some(c => c.parentId === country.parentId)) {
      this.continents.forEach(cont => {
        if (cont.id === country.parentId) cont.selected = false;
      });

      this.selectedContinents = this.selectedContinents.filter(sc => {
        return sc.id !== country.parentId;
      });

      this.countries = this.countries.filter(c => {
        return c.parentId !== country.parentId;
      });

      this.initialCountriesList = this.initialCountriesList.filter(c => {
        return c.parentId !== country.parentId;
      });
    }
  }

  onUsAllSelect(usAll: CountryCheckboxInterface): void {
    this.selectedGeographies = this.selectedGeographies.filter(sc => !sc.slug.includes(this.fragmentOfAllUsStatesSlug));

    this.initialCountriesList.forEach(ic => {
      if (ic.slug.includes(this.fragmentOfAllUsStatesSlug) && !ic.slug.includes(this.allUsSlug)) {
        ic.disabled = true;
        ic.selected = true;
      }
    });

    this.selectedGeographies.push(usAll);
  }

  removeRegion(region: CountryInterface) {
    // * country or state removing
    if (region.parentId !== 0) {
      this.countries.forEach(c => {
        if (c.id === region.id) c.selected = false;

        if (region.slug.includes(this.allUsSlug)) {
          c.disabled = false;
        }
      });

      this.selectedGeographies = this.selectedGeographies.filter(c => c.id !== region.id);

      // * remove continent if no countries from it is selected
      if (!this.selectedGeographies.some(sr => sr.parentId === region.parentId)) {
        this.selectedContinents = this.selectedContinents.filter(sc => {
          return sc.id !== region.parentId;
        });

        this.countries = this.countries.filter(c => c.parentId !== region.parentId);
        this.initialCountriesList = this.initialCountriesList.filter(c => c.parentId !== region.parentId);

        this.continents.forEach(c => {
          if (c.id === region.parentId) c.selected = false;
        });
      }
    }

    // * continent removing
    if (region.parentId === 0) {
      // * unselect continent
      this.continents.forEach(c => {
        if (c.id === region.id) c.selected = false;
      });

      // * remove continent and all its countries from selected regions
      this.selectedContinents = this.selectedContinents.filter(sc => {
        return sc.parentId !== region.id && region.id !== sc.id;
      });

      this.selectedGeographies = this.selectedGeographies.filter(c => c.id !== region.id);
      this.countries = this.countries.filter(c => c.parentId !== region.id);
      this.initialCountriesList = this.initialCountriesList.filter(c => c.parentId !== region.id);
    }
  }

  clearAllRegions() {
    this.continents?.forEach(c => (c.selected = false));
    this.countries = [];
    this.initialCountriesList = [];
    this.selectedContinents = [];
    this.selectedGeographies = [];
  }

  searchCountries(searchStr: any): void {
    this.countries = searchStr
      ? this.initialCountriesList?.filter(c => c.name.toLowerCase().includes(searchStr.toLowerCase()))
      : (this.countries = [...this.initialCountriesList]);
  }
}
