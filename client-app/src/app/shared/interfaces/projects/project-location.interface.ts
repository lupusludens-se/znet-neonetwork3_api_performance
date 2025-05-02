export interface ProjectLocationInterface {
  continent: string;
  country: string;
  state: string;
  city?: string;
  coordinates?: {
    latitude: string;
    longitude: string;
  };
}
