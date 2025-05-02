import { ControlContainer, FormControl, FormGroupDirective } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { filter } from 'rxjs/operators';
import { take } from 'rxjs';

import { CountryCheckboxInterface, CountryInterface } from '../../interfaces/user/country.interface';
import { GeographiesHelper } from '../../geographies-helpers/geographies.helper';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { CommonApiEnum } from '../../../core/enums/common-api.enum';
import { HttpService } from '../../../core/services/http.service';
import { RegionsService } from '../../services/regions.service';
import { EventsService } from '../../../+admin/services/events.service';

@Component({
  selector: 'neo-event-regions',
  templateUrl: 'event-regions.component.html',
  styleUrls: ['../../../../styles/regions-selection.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class EventRegionsComponent implements OnInit {
  @Input() invitedGeographies: CountryInterface[]; // * from edit event - already invited regions on creation of event
  @Input() editFlow: boolean;

  preSelectedGeographies: CountryInterface[]; // * for steps navigation?
  searchInput: FormControl = new FormControl();
  geographiesHelper = GeographiesHelper;
  commonApiEnum = CommonApiEnum;
  allUsSlug: string = 'us-all';
  fragmentOfAllUsStatesSlug: string = 'us-';
  continents: CountryCheckboxInterface[];
  selectedContinents: CountryCheckboxInterface[] = [];
  selectedGeographies: CountryInterface[] = [];
  countries: CountryCheckboxInterface[];
  initialCountriesList: CountryCheckboxInterface[];

  private selectedContinentsId: string;

  constructor(
    public regionsService: RegionsService,
    private httpService: HttpService,
    public controlContainer: ControlContainer,
    private eventsService: EventsService
  ) {}

  ngOnInit(): void {
    this.regionsService.continents$
      .pipe(
        filter(v => !!v),
        take(1)
      )
      .subscribe(c => {
        this.continents = [...c];

        this.eventsService.currentFormValue$
          .pipe(
            filter(v => !!v?.invitedRegions),
            take(1)
          )
          .subscribe(val => {
            this.preSelectedGeographies = val?.invitedRegions;

            this.preSelectedGeographies?.forEach(pc => {
              // * preselect continent
              if (pc.parentId === 0) {
                for (let i = 0; i < this.continents.length; i++) {
                  if (!this.continents[i].selected && this.continents[i].id === pc.id) {
                    this.continents[i].selected = true;
                    break;
                  }
                }
              }

              if (pc.parentId !== 0) {
                for (let i = 0; i < this.continents.length; i++) {
                  if (!this.continents[i].selected && this.continents[i].id === pc.parentId) {
                    this.continents[i].selected = true;
                    break;
                  }
                }
              }
            });

            this.selectedContinents = this.continents.filter(c => c.selected === true);
            this.selectedGeographies = [...val.invitedRegions];

            if (this.preSelectedGeographies && this.editFlow) {
              this.selectedContinents.forEach(sc => (sc.isInvited = true));
              this.selectedGeographies.forEach(sc => (sc.isInvited = true));
            }

            // * preselect country or state
            if (this.selectedGeographies.some(sr => sr.parentId !== 0)) {
              this.selectedContinentsId = this.selectedContinents.map(c => c.id).join(',');
              this.getCountries();
            }
          });
      });
    this.loadMore();
  }

  getOldPartialSelectedContinentsIds(): number[] {
    if (this.selectedGeographies?.length) {
      const oldPartialSelectedContinents = [];

      for (let j = 0; j < this.selectedGeographies.length; j++) {
        if (this.selectedGeographies[j].isInvited && this.selectedGeographies[j].parentId !== 0) {
          oldPartialSelectedContinents.push(
            this.selectedGeographies[j].parentId === 0
              ? this.selectedGeographies[j].id
              : this.selectedGeographies[j].parentId
          );
        }
      }

      return [...new Set(oldPartialSelectedContinents)];
    }

    return [];
  }

  getNewAndPartialSelectedContinentsIds(): number[] {
    if (this.selectedGeographies?.length) {
      const newAndPartialSelectedContinents = [];

      for (let j = 0; j < this.selectedGeographies.length; j++) {
        if (
          !this.selectedGeographies[j].isInvited ||
          (this.selectedGeographies[j].isInvited && this.selectedGeographies[j].parentId !== 0)
        ) {
          newAndPartialSelectedContinents.push(
            this.selectedGeographies[j].parentId === 0
              ? this.selectedGeographies[j].id
              : this.selectedGeographies[j].parentId
          );
        }
      }

      return [...new Set(newAndPartialSelectedContinents)];
    }

    return [];
  }

  getCountries(): void {
    this.selectedContinentsId = this.getNewAndPartialSelectedContinentsIds().join(',') || '';

    if (this.selectedContinentsId.length) {
      this.httpService
        .get<TagInterface[]>(this.commonApiEnum.Regions, {
          FilterBy: `parentids=${this.selectedContinentsId}`
        })
        .subscribe(c => {
          c.forEach(country => {
            this.selectedGeographies.forEach(c => {
              if (c.id === country.id) {
                country.selected = true;

                if (c.isInvited && this.editFlow) {
                  country.isInvited = true;
                }
              }
            });

            this.invitedGeographies?.forEach(ic => {
              this.selectedGeographies.forEach(sc => {
                if (ic.id === sc.id && !ic.selected) {
                  sc.isInvited = true;
                }
              });
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
        });
    }
  }

  loadMore(): void {
    this.searchInput.patchValue('');
    this.getCountries();
  }

  selectContinent(continent: CountryCheckboxInterface, index: number) {
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
    this.updateSelectedGeographiesRegions();
    if (this.continents[index].selected == true) {
      this.loadMore();
    }
  }

  searchCountries(searchStr: string): void {
    this.countries = searchStr
      ? this.initialCountriesList?.filter(c => c.name.toLowerCase().includes(searchStr.toLowerCase()))
      : [...this.initialCountriesList];
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

      this.countries = this.countries.filter(c => c.parentId !== country.parentId);
      this.initialCountriesList = this.initialCountriesList.filter(ic => ic.parentId !== country.parentId);
    }

    this.updateSelectedGeographiesRegions();
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

  clearAllSelectedCountries(): void {
    if (this.selectedGeographies.every(sg => !sg.isInvited)) {
      this.countries = [];
      this.initialCountriesList = [];
    } else {
      let countries = [];

      const oldPartialContinentsIds = this.getOldPartialSelectedContinentsIds();

      countries = [
        ...countries,
        ...this.countries.filter(country =>
          oldPartialContinentsIds.some(opc => opc === country.id || opc === country.parentId)
        )
      ];

      this.countries = countries;
      this.initialCountriesList = [...this.countries];

      this.countries.forEach(c => {
        if (this.selectedGeographies.some(sg => !sg.isInvited && sg.id === c.id)) {
          c.selected = false;
        }
      });
    }

    this.continents?.forEach(c => {
      if (!c.isInvited) {
        c.selected = false;
      }
    });

    this.selectedGeographies = this.selectedGeographies.filter(sc => sc.isInvited);
    this.selectedContinents = this.selectedContinents.filter(sc => sc.isInvited);
    this.preSelectedGeographies = [];

    this.updateSelectedGeographiesRegions();
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

        this.countries = this.countries?.filter(c => c.parentId !== region.parentId);
        this.initialCountriesList = this.initialCountriesList?.filter(c => c.parentId !== region.parentId);

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
      this.countries = this.countries?.filter(c => c.parentId !== region.id);
      this.initialCountriesList = this.initialCountriesList?.filter(c => c.parentId !== region.parentId);
    }

    this.updateSelectedGeographiesRegions();
  }

  updateSelectedGeographiesRegions() {
    this.controlContainer.control.get('invitedRegions')?.patchValue(this.selectedGeographies ?? null);

    this.eventsService.updateFormValue({
      invitedRegions: this.selectedGeographies ?? null
    });
  }

  calculateClearButtonVisibility(): boolean {
    return this.selectedGeographies.length > 0 && !this.selectedGeographies.every(pg => pg.isInvited);
  }
}
