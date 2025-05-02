import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { ControlContainer, FormGroupDirective, ValidationErrors } from '@angular/forms';
import * as MapboxGeocoder from '@mapbox/mapbox-gl-geocoder';
import * as mapboxgl from 'mapbox-gl';
import * as dayjs from 'dayjs';

import { ProjectOffsitePpaInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from '../../../../shared/interfaces/projects/project.interface';
import { ProjectTypesSteps } from '../../../+add-project/enums/project-types-name.enum';
import { environment } from '../../../../../environments/environment';
import { HttpService } from '../../../../core/services/http.service';

@Component({
  selector: 'neo-edit-project-ppa',
  templateUrl: 'edit-project-ppa.component.html',
  styleUrls: ['../../edit-project.component.scss', 'edit-project-ppa.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class EditProjectPpaComponent implements OnInit, AfterViewInit {
  @Input() project: ProjectInterface;
  @Input() formSubmitted: boolean;
  @Input() upsideShareError: ValidationErrors | null;
  isoList: TagInterface[] = [
    { name: 'None', id: null },
    { name: 'PJM', id: 1 },
    { name: 'ISO-NE', id: 2 },
    { name: 'MISO', id: 3 },
    { name: 'SERC', id: 4 },
    { name: 'SPP', id: 5 },
    { name: 'ERCOT', id: 6 },
    { name: 'WECC', id: 7 },
    { name: 'CAISO', id: 8 },
    { name: 'AESO', id: 9 },
    { name: 'NYISO', id: 10 },
    { name: 'Other', id: 11 }
  ];
  projectTypeList: TagInterface[] = [
    { name: 'Energy only', id: 1 },
    { name: 'Energy with project EACs', id: 2 },
    { name: 'Energy with certified swap EACs', id: 3 },
    { name: 'Retail delivered product', id: 4 }
  ];
  offtakerValueList: TagInterface[] = [
    { name: 'Renewable Attributes', id: 6 },
    { name: 'Energy Arbitrage', id: 7 },
    { name: 'Visible Commitment to Climate Action', id: 8 },
    { name: 'Community Benefits', id: 9 }
  ];
  minDate: Date;
  map: mapboxgl.Map;
  geocoder: MapboxGeocoder;
  showPublicPart: boolean = true;
  showPrivatePart: boolean;
  projectTypes = ProjectTypesSteps;

  constructor(public controlContainer: ControlContainer, private readonly httpService: HttpService) {}

  ngOnInit(): void {
    const projectDetails: ProjectOffsitePpaInterface = this.project.projectDetails as ProjectOffsitePpaInterface;

    this.controlContainer.control.patchValue({
      latitude: projectDetails.latitude,
      longitude: projectDetails.longitude,
      isoRto: this.isoList.filter(i => i.id === projectDetails.isoRtoId)[0] ?? '',
      productType: this.projectTypeList.filter(p => p.id === projectDetails.productTypeId)[0] ?? '',
      commercialOperationDate: projectDetails.commercialOperationDate
        ? dayjs(projectDetails.commercialOperationDate).format('YYYY-MM-DD')
        : '',
      valuesToOfftakers: projectDetails.valuesToOfftakers,
      ppaTermYearsLength: projectDetails.ppaTermYearsLength,
      totalProjectNameplateMWACCapacity: projectDetails.totalProjectNameplateMWACCapacity,
      totalProjectExpectedAnnualMWhProductionP50: projectDetails.totalProjectExpectedAnnualMWhProductionP50,
      minimumOfftakeMWhVolumeRequired: projectDetails.minimumOfftakeMWhVolumeRequired,
      notesForPotentialOfftakers: projectDetails.notesForPotentialOfftakers,
      productTypeId: projectDetails.productTypeId,
      isoRtoId: projectDetails.isoRtoId
    });

    if (projectDetails.valuesToOfftakers) {
      projectDetails.valuesToOfftakers.forEach(vto => {
        this.offtakerValueList.map(v => {
          if (v.id === vto.id) {
            v.selected = true;
          }
        });
      });
    }

    this.setControlValue(projectDetails.isoRtoId, 'isoRtoId');
    this.setControlValue(projectDetails.productTypeId, 'productTypeId');
  }
  ngAfterViewInit(): void {
    this.setupMap();

    const projectDetails: ProjectOffsitePpaInterface = this.project.projectDetails as ProjectOffsitePpaInterface;

    this.map.flyTo({
      center: {
        lat: projectDetails.latitude,
        lng: projectDetails.longitude
      }
    });

    this.httpService
      .getAny(
        `${environment.mapBox.api}/mapbox.places/${projectDetails.longitude},${projectDetails.latitude}.json?access_token=${environment.mapBox.accessToken}`
      )
      .subscribe((res: any) => {
        (document.getElementsByClassName('mapboxgl-ctrl-geocoder--input')[0] as any).value = res.features.find(
          f => f.id.includes('address') || f.id.includes('place')
        ).place_name;
      });
  }

  private setupMap() {
    const projectDetails: ProjectOffsitePpaInterface = this.project.projectDetails as ProjectOffsitePpaInterface;
    this.map = new mapboxgl.Map({
      accessToken: environment.mapBox.accessToken,
      container: 'map',
      style: environment.mapBox.styles,
      zoom: 12,
      center: [projectDetails.longitude, projectDetails.latitude]
    });

    new mapboxgl.Marker().setLngLat([projectDetails.longitude, projectDetails.latitude]).addTo(this.map);

    this.map.addControl(new mapboxgl.NavigationControl());
    this.geocoder = new MapboxGeocoder({
      accessToken: environment.mapBox.accessToken,
      mapboxgl: mapboxgl,
      marker: true
    });

    this.map.addControl(this.geocoder, 'top-left');
    this.geocoder.on('result', e => {
      this.controlContainer.control.patchValue({
        longitude: e.result.geometry.coordinates[0],
        latitude: e.result.geometry.coordinates[1]
      });
    });
  }

  chooseValuesToOfftakers(val: TagInterface, unselect: boolean) {
    if (unselect) {
      const updatedVal: number[] = [...this.controlContainer.control.value.valuesToOfftakers].filter(
        o => o.id !== val.id
      );

      this.controlContainer.control.patchValue({
        valuesToOfftakers: [...updatedVal]
      });
    } else {
      this.controlContainer.control.patchValue({
        valuesToOfftakers: [...this.controlContainer.control.value.valuesToOfftakers, val]
      });
    }
  }

  setControlValue(dropdownValId: number, controlName: string) {
    this[controlName] = dropdownValId;
    this.controlContainer.control.patchValue({ [controlName]: dropdownValId });
  }
}
