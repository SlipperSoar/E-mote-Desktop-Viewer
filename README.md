# E-mote-Desktop-Viewer

一个使用Unity开发的E-mote的简易桌面老婆（bushi）

## 使用的插件/SDK

- E-mote官方的Unity SDK（Type2）
- FreeMote ToolKit的插件（主要用于解读E-mote的psb文件）

## 当前问题

Editor下正常，Build（win64）后报错：

> <font color=#ff0000>NotSupportedException: Interface 'FreeMote.Plugins.IPsbPluginInfo' is not a valid MetadataView; MetadataViews do not support non-public interfaces, and interfaces that contain members that are not properties.</font>
> System.ComponentModel.Composition.MetadataViewProvider.GetMetadataView[TMetadataView] (System.Collections.Generic.IDictionary`2[TKey,TValue] metadata) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.AttributedModelServices.GetMetadataView[TMetadataView] (System.Collections.Generic.IDictionary`2[TKey,TValue] metadata) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.ExportServices.CreateStronglyTypedLazyOfTM[T,M] (System.ComponentModel.Composition.Primitives.Export export) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.ReflectionModel.ImportingItem.CastSingleExportToImportType (System.Type type, System.ComponentModel.Composition.Primitives.Export export) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.ReflectionModel.ImportingItem.CastExportsToCollectionImportType (System.ComponentModel.Composition.Primitives.Export[] exports) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.ReflectionModel.ImportingItem.CastExportsToImportType (System.ComponentModel.Composition.Primitives.Export[] exports) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.ReflectionModel.ReflectionComposablePart.SetImport (System.ComponentModel.Composition.ReflectionModel.ImportingItem item, System.ComponentModel.Composition.Primitives.Export[] exports) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.ReflectionModel.ReflectionComposablePart.SetImport (System.ComponentModel.Composition.Primitives.ImportDefinition definition, System.Collections.Generic.IEnumerable`1[T] exports) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.Hosting.ImportEngine+PartManager.TrySetImport (System.ComponentModel.Composition.Primitives.ImportDefinition import, System.Collections.Generic.IEnumerable`1[T] exports) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.Hosting.ImportEngine.TrySatisfyImportSubset (System.ComponentModel.Composition.Hosting.ImportEngine+PartManager partManager, System.Collections.Generic.IEnumerable`1[T] imports, System.ComponentModel.Composition.Hosting.AtomicComposition atomicComposition) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.Hosting.ImportEngine.TrySatisfyImportsStateMachine (System.ComponentModel.Composition.Hosting.ImportEngine+PartManager partManager, System.ComponentModel.Composition.Primitives.ComposablePart part) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.Hosting.ImportEngine.TrySatisfyImports (System.ComponentModel.Composition.Hosting.ImportEngine+PartManager partManager, System.ComponentModel.Composition.Primitives.ComposablePart part, System.Boolean shouldTrackImports) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.Hosting.ImportEngine.SatisfyImports (System.ComponentModel.Composition.Primitives.ComposablePart part) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.Hosting.ComposablePartExportProvider+<>c__DisplayClass19_0.<Compose>b__0 () (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.Hosting.CompositionServices.TryInvoke (System.Action action) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.Hosting.ComposablePartExportProvider.Compose (System.ComponentModel.Composition.Hosting.CompositionBatch batch) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.Hosting.CompositionContainer.Compose (System.ComponentModel.Composition.Hosting.CompositionBatch batch) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> System.ComponentModel.Composition.AttributedModelServices.ComposeParts (System.ComponentModel.Composition.Hosting.CompositionContainer container, System.Object[] attributedParts) (at <b57eb26b8b9f4dff90f04ec5880a0f28>:0)
> FreeMote.Plugins.FreeMount.InitPlugins (System.String path, System.String dllDirPath) (at <a0b8e18589f44a53b2adc30fe781fb4e>:0)
> FreeMote.Plugins.FreeMount.Init (System.String path, System.String dllDirPath) (at <a0b8e18589f44a53b2adc30fe781fb4e>:0)
> PSBImporter..cctor () (at <b74bf95089de4b23bf9cc6d9523e55f5>:0)
> Rethrow as TypeInitializationException: The type initializer for 'PSBImporter' threw an exception.
> EmoteLoader.Start () (at <b74bf95089de4b23bf9cc6d9523e55f5>:0)

