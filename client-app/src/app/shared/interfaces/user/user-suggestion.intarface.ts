export interface UserSuggestionInterface {
  suggestions: Suggestions[];
}

export interface Suggestions {
  id: string;
  type: number;
  name: string;
}
