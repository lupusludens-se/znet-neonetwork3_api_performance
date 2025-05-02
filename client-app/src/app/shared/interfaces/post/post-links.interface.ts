export interface PostLinksInterface {
  self: { href: string }[];
  collection: { href: string }[];
  about: { href: string }[];
  author: { embeddable: boolean; href: string }[];
  replies: { embeddable: boolean; href: string }[];
  'version-history': { count: number; href: string }[];
  'predecessor-version': { id: number; href: string }[];
  'wp:attachment': { href: string }[];
  'wp:term': { taxonomy: string; embeddable: boolean; href: string }[];
  curies: { name: string; href: string }[];
}
