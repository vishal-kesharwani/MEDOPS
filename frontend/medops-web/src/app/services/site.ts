import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AddressDto {
  street: string;
  city: string;
  state: string;
  country: string;
  zipCode: string;
}

export interface ContactInfoDto {
  email: string;
  phone: string;
}

export interface SiteDto {
  id: string;
  name: string;
  description: string;
  status: string;
  address: AddressDto;
  contactInfo: ContactInfoDto;
  createdBy: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateSiteDto {
  name: string;
  description: string;
  address: AddressDto;
  contactInfo: ContactInfoDto;
}

@Injectable({ providedIn: 'root' })
export class Site {
  private readonly apiUrl = '/api/sites';

  constructor(private http: HttpClient) {}

  getAll(): Observable<SiteDto[]> {
    return this.http.get<SiteDto[]>(this.apiUrl);
  }

  getById(id: string): Observable<SiteDto> {
    return this.http.get<SiteDto>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateSiteDto): Observable<SiteDto> {
    return this.http.post<SiteDto>(this.apiUrl, dto);
  }

  update(id: string, dto: CreateSiteDto): Observable<SiteDto> {
    return this.http.put<SiteDto>(`${this.apiUrl}/${id}`, dto);
  }

  deactivate(id: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/deactivate`, {});
  }

  activate(id: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/activate`, {});
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
