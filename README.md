# 📸 Image Processing API

## 📚 Overview

This is a **.NET 8** Web API for handling **image uploads, processing, and metadata extraction**.  
It is **fully dynamic**, allowing new image formats and sizes to be added easily using **enums**.

### **Key Features**

- ✅ Supports multiple image formats (configurable via `AllowedImageFormats` enum).
- ✅ Converts all images to **WebP format** automatically.
- ✅ Generates multiple **resized versions** dynamically (configurable via `ImageSize` enum).
- ✅ Extracts **EXIF metadata** and stores it **in a structured JSON file**.
- ✅ **Logs every request and response** to a log file.
- ✅ Allows **easy expansion** with new formats and sizes.

---

## 🚀 **Getting Started**

### **1️⃣ Prerequisites**

- Install **.NET 8 SDK**
- Ensure **ASP.NET Core Runtime** is installed

### **2️⃣ Clone the Repository**

```sh
git clone https://github.com/Beshoy-Grace/Image-Processing-API.git
cd image-api
```

### **3️⃣ Install Dependencies**

```sh
dotnet restore
```

### **4️⃣ Build & Run**

```sh
dotnet run
```

The API will be available at **[http://localhost:5000/](http://localhost:5000/)**.

---

## 🛠 **API Endpoints**

### 🔹 **1. Upload Images**

```http
POST /image/upload
```

#### **Request**

- `multipart/form-data`
- Accepts multiple files (`.jpg`, `.png`, `.webp` - configurable)
- Max file size **2MB**

#### **Response**

```json
{
  "isSuccess": true,
  "message": "Images uploaded successfully.",
  "responseData": [
    { "id": "abc123" },
    { "id": "def456" }
  ],
  "errors": null,
  "statusCode": 200
}
```

---

### 🔹 **2. Download Resized Image**

```http
GET /image/download/{id}/{size}
```

#### **Parameters**

- `id`: Unique image ID.
- `size`: `phone | tablet | desktop` (can be expanded dynamically).

#### **Response**

- Returns WebP image file.

---

### 🔹 **3. Get Image Metadata**

```http
GET /image/metadata/{id}
```

#### **Response**

```json
{
  "isSuccess": true,
  "message": "Metadata found.",
  "responseData": {
    "Exif IFD0": {
      "Make": "Canon",
      "Model": "Canon EOS 5D Mark III"
    },
    "GPS": {
      "Latitude": "37.7749° N",
      "Longitude": "122.4194° W"
    }
  },
  "errors": null,
  "statusCode": 200
}
```

---

### 🔹 **4. Get Log File**

```http
GET /log
```

#### **Response**

- Returns the log file containing all API requests & responses.

---

## 🏰 **Architecture & Components**

### ✅ **1. BaseCommandResponse**

A generic class used to format API responses:

```csharp
public class BaseCommandResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T ResponseData { get; set; }
    public List<string> Errors { get; set; }
    public int StatusCode { get; set; }
}
```

---

### ✅ **2. Dynamic Image Sizes (Enum)**

The system supports multiple image sizes dynamically:

```csharp
public enum ImageSize
{
    Phone = 640,
    Tablet = 1024,
    Desktop = 1920
}
```

- 🎯 **New sizes can be added easily** by updating the `ImageSize` enum.

---

### ✅ **3. Dynamic Allowed Image Formats (Enum)**

Allowed image formats are controlled via an enum:

```csharp
public static class AllowedImageFormats
{
    public static readonly HashSet<string> Formats = new() { ".jpg", ".jpeg", ".png", ".webp" };
}
```

- 🎯 **To support more formats, just add them to this list.**

---

### ✅ **4. Logging System**

- Every request and response is logged into a file.
- Logs are stored in **logs/log-yyyy-MM-dd.txt**.
- Can be accessed via the `/log` endpoint.

#### **Example Log Entry**

```
[2025-03-15 10:30:22] - Request: POST /image/upload
[2025-03-15 10:30:22] - Response: { "isSuccess": true, "message": "Images uploaded successfully." }
```

---

## 💡 **Future Enhancements**

- Add support for **GIF and BMP** formats.
- Implement a **frontend UI** for image management.
- Improve **metadata parsing for additional formats**.

---

## 🛠 **Troubleshooting**

- Ensure **.NET 8 SDK** is installed.
- Check logs for errors: `logs/log-yyyy-MM-dd.txt`
- If metadata is empty, ensure the image contains **EXIF data**.

---
